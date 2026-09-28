using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Pricing;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Checkout.Dtos;
using Workspace_Management_System.Application.Services.Pricing;
using Workspace_Management_System.Domain.Enums;
using Workspace_Management_System.Domain.Models;

namespace Workspace_Management_System.Application.Features.Checkout.Commands.CheckoutSession
{
    public class CheckoutSessionHandler
        : IRequestHandler<CheckoutSessionCommand, CheckoutResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPricingCalculator _pricingCalculator;
        private readonly ICurrentUserService _currentUser;

        public CheckoutSessionHandler(
            IUnitOfWork unitOfWork,
            IPricingCalculator pricingCalculator,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _pricingCalculator = pricingCalculator;
            _currentUser = currentUser;
        }

        public async Task<CheckoutResponseDto> Handle(
            CheckoutSessionCommand request,
            CancellationToken cancellationToken)
        {
            var dbTransaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                var now = DateTime.UtcNow;

                var session = await _unitOfWork.Sessions
                    .Query()
                    .Include(x => x.Workspace)
                    .Include(x => x.SessionProducts)
                        .ThenInclude(x => x.Product)
                    .FirstOrDefaultAsync(
                        x => x.Id == request.SessionId,
                        cancellationToken);

                if (session is null)
                {
                    throw new KeyNotFoundException(
                        $"Session with ID {request.SessionId} was not found.");
                }

                if (session.Status != SessionStatus.Active)
                {
                    throw new InvalidOperationException(
                        "Only active sessions can be checked out.");
                }

                var endTime = now;

                if (endTime < session.StartTime)
                {
                    throw new InvalidOperationException(
                        "Checkout time cannot be before session start time.");
                }

                session.EndTime = endTime;

                var duration = endTime - session.StartTime;

                var pricingRules = await _unitOfWork.PricingRules
                    .Query()
                    .Where(x =>
                        x.PricingPlanId == session.PricingPlanId &&
                        x.WorkspaceTypeId == session.Workspace.WorkspaceTypeId &&
                        x.IsActive &&
                        (!x.StartDate.HasValue || x.StartDate <= endTime) &&
                        (!x.EndDate.HasValue || x.EndDate >= endTime))
                    .ToListAsync(cancellationToken);

                if (pricingRules.Count == 0)
                {
                    throw new InvalidOperationException(
                        "No applicable pricing rules were found for this session.");
                }

                var pricingInput = new PricingCalculationInput
                {
                    StartTime = session.StartTime,
                    EndTime = endTime,

                    HourlyRate = pricingRules
                        .FirstOrDefault(x =>
                            x.RuleType == PricingRuleType.HourlyRate)
                        ?.Value ?? 0,

                    HalfHourRate = pricingRules
                        .FirstOrDefault(x =>
                            x.RuleType == PricingRuleType.HalfHourRate)
                        ?.Value,

                    MinimumCharge = pricingRules
                        .FirstOrDefault(x =>
                            x.RuleType == PricingRuleType.MinimumCharge)
                        ?.Value ?? 0,

                    FullDayMaximum = pricingRules
                        .FirstOrDefault(x =>
                            x.RuleType == PricingRuleType.FullDayMaximum)
                        ?.Value,

                    RoundingMinutes = 0,
                    RoundingMode = RoundingMode.None
                };

                var pricingResult =
                    _pricingCalculator.Calculate(pricingInput);

                var workspaceAmount = pricingResult.FinalAmount;

                var productsAmount = 0m;

                var responseItems = new List<CheckoutResponseItemDto>();

                var transaction = new Transaction
                {
                    SessionId = session.Id,
                    CustomerId = session.CustomerId,
                    EmployeeId = session.EmployeeId,

                    TransactionNumber =
                        $"TRX-{now:yyyyMMddHHmmssfff}",

                    Status = "Completed",

                    CreatedAt = now,
                    CreatedBy = _currentUser.UserId
                };


                transaction.Items.Add(new TransactionItem
                {
                    ItemType = "Workspace",
                    Description = "Workspace Usage",
                    Quantity = 1,
                    UnitPrice = workspaceAmount,
                    Total = workspaceAmount,

                    CreatedAt = now,
                    CreatedBy = _currentUser.UserId
                });

                responseItems.Add(new CheckoutResponseItemDto
                {
                    Description = "Workspace Usage",
                    ItemType = "Workspace",
                    Quantity = 1,
                    UnitPrice = workspaceAmount,
                    Total = workspaceAmount
                });

                foreach (var sessionProduct in session.SessionProducts)
                {
                    if (sessionProduct.Product is null)
                    {
                        throw new InvalidOperationException(
                            $"Product with ID {sessionProduct.ProductId} could not be loaded.");
                    }

                    var itemTotal =
                        sessionProduct.Quantity *
                        sessionProduct.UnitPrice;

                    productsAmount += itemTotal;

                    transaction.Items.Add(new TransactionItem
                    {
                        ItemType = "Product",
                        ProductId = sessionProduct.ProductId,
                        Description = sessionProduct.Product.EnglishName,
                        Quantity = sessionProduct.Quantity,
                        UnitPrice = sessionProduct.UnitPrice,
                        Total = itemTotal,

                        CreatedAt = now,
                        CreatedBy = _currentUser.UserId
                    });

                    responseItems.Add(new CheckoutResponseItemDto
                    {
                        Description = sessionProduct.Product.EnglishName,
                        ItemType = "Product",
                        Quantity = sessionProduct.Quantity,
                        UnitPrice = sessionProduct.UnitPrice,
                        Total = itemTotal
                    });
                }

                var subtotal =
                    workspaceAmount +
                    productsAmount;

                var discountAmount =
                    request.Request.DiscountAmount;

                if (discountAmount < 0)
                {
                    throw new InvalidOperationException(
                        "Discount cannot be negative.");
                }

                if (discountAmount > subtotal)
                {
                    throw new InvalidOperationException(
                        "Discount cannot be greater than subtotal.");
                }

                if (request.Request.TaxRate < 0)
                {
                    throw new InvalidOperationException(
                        "Tax rate cannot be negative.");
                }

                var taxableAmount =
                    subtotal - discountAmount;

                var taxAmount =
                    taxableAmount *
                    (request.Request.TaxRate / 100m);

                var total =
                    taxableAmount + taxAmount;

                transaction.Subtotal = subtotal;
                transaction.TaxAmount = taxAmount;
                transaction.Total = total;
                session.Status = SessionStatus.Completed;
                session.UpdatedAt = now;
                session.UpdatedBy = _currentUser.UserId;

                _unitOfWork.Sessions.Update(session);

                await _unitOfWork.Transactions.AddAsync(transaction);

                await _unitOfWork.SaveAsync();

                await dbTransaction.CommitAsync(cancellationToken);

                return new CheckoutResponseDto
                {
                    TransactionId = transaction.Id,
                    TransactionNumber = transaction.TransactionNumber,
                    SessionId = session.Id,
                    StartTime = session.StartTime,
                    EndTime = endTime,
                    Duration = duration,
                    WorkspaceAmount = workspaceAmount,
                    ProductsAmount = productsAmount,
                    Subtotal = subtotal,
                    DiscountAmount = discountAmount,
                    TaxAmount = taxAmount,
                    Total = total,
                    Status = transaction.Status,
                    Items = responseItems
                };
            }
            catch
            {
                await dbTransaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}