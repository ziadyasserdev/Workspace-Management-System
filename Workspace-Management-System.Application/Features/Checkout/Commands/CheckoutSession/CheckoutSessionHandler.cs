using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Contracts;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Pricing;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Checkout.Dtos;
using Workspace_Management_System.Application.Resources;
using Workspace_Management_System.Application.Services.Pricing;
using Workspace_Management_System.Domain.Enums;
using Workspace_Management_System.Domain.Models;

namespace Workspace_Management_System.Application.Features.Checkout.Commands.CheckoutSession;

public class CheckoutSessionHandler
    : IRequestHandler<CheckoutSessionCommand, CheckoutResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPricingCalculator _pricingCalculator;
    private readonly ICurrentUserService _currentUser;
    private readonly ILocalizationService _localizationService;
    private readonly IStringLocalizer _localizer;

    public CheckoutSessionHandler(
        IUnitOfWork unitOfWork,
        IPricingCalculator pricingCalculator,
        ICurrentUserService currentUser,
        ILocalizationService localizationService,
        IStringLocalizerFactory factory)
    {
        _unitOfWork = unitOfWork;
        _pricingCalculator = pricingCalculator;
        _currentUser = currentUser;
        _localizationService = localizationService;
        _localizer = factory.Create(typeof(SharedResources));
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
                    _localizer["SessionNotFoundWithId", request.SessionId]);
            }

            if (session.Status != SessionStatus.Active)
            {
                throw new InvalidOperationException(
                    _localizer["OnlyActiveSessionsCanBeCheckedOut"]);
            }

            var endTime = now;

            if (endTime < session.StartTime)
            {
                throw new InvalidOperationException(
                    _localizer["CheckoutTimeCannotBeBeforeSessionStart"]);
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
                        _localizer[
                            "InsufficientPackageHours",
                            remainingHours,
                            sessionHours]);
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
                        _localizer["NoApplicablePricingRules"]);
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
                Status = TransactionStatus.Completed.ToString(),
                CreatedAt = now,
                CreatedBy = _currentUser.UserId
            };

            if (workspaceAmount > 0)
            {
                transaction.Items.Add(new TransactionItem
                {
                    ItemType = ItemType.Workspace.ToString(),
                    Description = _localizer["WorkspaceUsage"],
                    Quantity = 1,
                    UnitPrice = workspaceAmount,
                    Total = workspaceAmount,
                    CreatedAt = now,
                    CreatedBy = _currentUser.UserId
                });

                responseItems.Add(new CheckoutResponseItemDto
                {
                    Description = _localizer["WorkspaceUsage"],
                    ItemType = _localizer[
                        $"ItemType_{ItemType.Workspace}"],
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
                        _localizer[
                            "ProductCouldNotBeLoaded",
                            sessionProduct.ProductId]);
                }

                var itemTotal =
                    sessionProduct.Quantity *
                    sessionProduct.UnitPrice;

                productsAmount += itemTotal;

                var productName =
                    _localizationService.GetLocalizedValue(
                        sessionProduct.Product.NameEn,
                        sessionProduct.Product.NameAr);

                transaction.Items.Add(new TransactionItem
                {
                    ItemType = ItemType.Product.ToString(),
                    ProductId = sessionProduct.ProductId,
                    Description = productName,
                    Quantity = sessionProduct.Quantity,
                    UnitPrice = sessionProduct.UnitPrice,
                    Total = itemTotal,
                    CreatedAt = now,
                    CreatedBy = _currentUser.UserId
                });

                responseItems.Add(new CheckoutResponseItemDto
                {
                    Description = productName,
                    ItemType = _localizer[
                        $"ItemType_{ItemType.Product}"],
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
                        _localizer[
                            "ServiceCouldNotBeLoaded",
                            sessionService.ServiceId]);
                }

                var itemTotal =
                    sessionService.Quantity *
                    sessionService.UnitPrice;

                servicesAmount += itemTotal;

                var serviceName =
                    _localizationService.GetLocalizedValue(
                        sessionService.Service.NameEn,
                        sessionService.Service.NameAr);

                transaction.Items.Add(new TransactionItem
                {
                    ItemType = ItemType.Service.ToString(),
                    Description = serviceName,
                    Quantity = sessionService.Quantity,
                    UnitPrice = sessionService.UnitPrice,
                    Total = itemTotal,
                    CreatedAt = now,
                    CreatedBy = _currentUser.UserId
                });

                responseItems.Add(new CheckoutResponseItemDto
                {
                    Description = serviceName,
                    ItemType = _localizer[
                        $"ItemType_{ItemType.Service}"],
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
                        _localizer["DiscountNotFound"]);
                }

                if (!discount.IsActive)
                {
                    throw new InvalidOperationException(
                        _localizer["DiscountIsNotActive"]);
                }

                if (discount.Value < 0)
                {
                    throw new InvalidOperationException(
                        _localizer["DiscountValueCannotBeNegative"]);
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
                            _localizer[
                                "DiscountPercentageCannotExceed100"]);
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
                    _localizer["TaxRateCannotBeNegative"]);
            }

            if (taxRate > 100)
            {
                throw new InvalidOperationException(
                    _localizer["TaxRateCannotExceed100"]);
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
                Status = _localizer[
                    $"TransactionStatus_{TransactionStatus.Completed}"],
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
