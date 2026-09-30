using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.Metrics;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Pricing;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Checkout.Dtos;
using Workspace_Management_System.Application.Services.Pricing;
using Workspace_Management_System.Domain.Enums;
using Workspace_Management_System.Domain.Models;
using static System.Net.Mime.MediaTypeNames;

namespace Workspace_Management_System.Application.Features.Checkout.Commands.CheckoutSession;

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
                .Include(x => x.SessionServices)
                    .ThenInclude(x => x.Service)
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

            var sessionHours = (decimal)duration.TotalHours;

            var customerPackage = await _unitOfWork.CustomerPackages
                .Query()
                .FirstOrDefaultAsync(
                    x =>
                        x.CustomerId == session.CustomerId &&
                        x.Status == CustomerPackageStatus.Active &&
                        !x.IsDeleted &&
                        x.RemainingHours.HasValue &&
                        x.RemainingHours.Value > 0 &&
                        x.StartDate <= endTime &&
                        (!x.EndDate.HasValue || x.EndDate.Value >= endTime),
                    cancellationToken);

            var workspaceAmount = 0m;
            var packageHoursUsed = 0m;

            if (customerPackage is not null)
            {
                var remainingHours =
                    customerPackage.RemainingHours!.Value;

                if (remainingHours < sessionHours)
                {
                    throw new InvalidOperationException(
                        $"Customer has only {remainingHours} package hours remaining, " +
                        $"but this session requires {sessionHours:F2} hours.");
                }

                packageHoursUsed = sessionHours;

                customerPackage.RemainingHours =
                    remainingHours - packageHoursUsed;

                customerPackage.UpdatedAt = now;
                customerPackage.UpdatedBy = _currentUser.UserId;

                if (customerPackage.RemainingHours <= 0)
                {
                    customerPackage.RemainingHours = 0;
                    customerPackage.Status = CustomerPackageStatus.Expired;
                    customerPackage.EndDate ??= now;
                }
            }
            else
            {
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

                workspaceAmount = pricingResult.FinalAmount;
            }

            var productsAmount = 0m;
            var servicesAmount = 0m;

            var responseItems =
                new List<CheckoutResponseItemDto>();

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

            if (workspaceAmount > 0)
            {
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
            }

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

            foreach (var sessionService in session.SessionServices)
            {
                if (sessionService.Service is null)
                {
                    throw new InvalidOperationException(
                        $"Service with ID {sessionService.ServiceId} could not be loaded.");
                }

                var itemTotal =
                    sessionService.Quantity *
                    sessionService.UnitPrice;

                servicesAmount += itemTotal;

                transaction.Items.Add(new TransactionItem
                {
                    ItemType = "Service",
                    Description = sessionService.Service.Name,
                    Quantity = sessionService.Quantity,
                    UnitPrice = sessionService.UnitPrice,
                    Total = itemTotal,

                    CreatedAt = now,
                    CreatedBy = _currentUser.UserId
                });

                responseItems.Add(new CheckoutResponseItemDto
                {
                    Description = sessionService.Service.Name,
                    ItemType = "Service",
                    Quantity = sessionService.Quantity,
                    UnitPrice = sessionService.UnitPrice,
                    Total = itemTotal
                });
            }

            var subtotal =
                workspaceAmount +
                productsAmount +
                servicesAmount;

            decimal discountAmount = 0m;

            if (request.Request.DiscountId.HasValue)
            {
                var discount = await _unitOfWork.Discounts
                    .GetByIdAsync(request.Request.DiscountId.Value);

                if (discount is null)
                {
                    throw new KeyNotFoundException(
                        "Discount not found.");
                }

                if (!discount.IsActive)
                {
                    throw new InvalidOperationException(
                        "Discount is not active.");
                }

                if (discount.Value < 0)
                {
                    throw new InvalidOperationException(
                        "Discount value cannot be negative.");
                }

                if (discount.DiscountType == DiscountType.FixedAmount)
                {
                    discountAmount = discount.Value;
                }
                else if (discount.DiscountType == DiscountType.Percentage)
                {
                    if (discount.Value > 100)
                    {
                        throw new InvalidOperationException(
                            "Discount percentage cannot be greater than 100.");
                    }

                    discountAmount =
                        subtotal * (discount.Value / 100m);
                }

                if (discountAmount > subtotal)
                {
                    discountAmount = subtotal;
                }
            }

            var taxRate =
                request.Request.TaxRate ?? 0m;

            if (taxRate < 0)
            {
                throw new InvalidOperationException(
                    "Tax rate cannot be negative.");
            }

            if (taxRate > 100)
            {
                throw new InvalidOperationException(
                    "Tax rate cannot be greater than 100.");
            }

            var taxableAmount =
                subtotal - discountAmount;

            var taxAmount =
                taxableAmount * (taxRate / 100m);

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
                ServicesAmount = servicesAmount,

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