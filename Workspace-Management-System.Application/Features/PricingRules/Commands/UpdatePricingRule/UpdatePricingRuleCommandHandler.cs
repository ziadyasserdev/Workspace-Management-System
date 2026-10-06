using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Resources;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.PricingRule.Commands.UpdatePricingRule
{
    public class UpdatePricingRuleCommandHandler
        : IRequestHandler<UpdatePricingRuleCommand, Result<bool>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUser;
        private readonly IStringLocalizer _localizer;

        public UpdatePricingRuleCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser,
            IStringLocalizerFactory factory)
        {
            this.unitOfWork = unitOfWork;
            this.currentUser = currentUser;
            _localizer = factory.Create(typeof(SharedResources));
        }

        public async Task<Result<bool>> Handle(
            UpdatePricingRuleCommand request,
            CancellationToken cancellationToken)
        {
            var pricingRule = await unitOfWork.PricingRules
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id &&
                         !x.IsDeleted,
                    cancellationToken);

            if (pricingRule == null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    _localizer["PricingRuleNotFound"]);
            }

            var pricingPlanExists = await unitOfWork.PricingPlans
                .Query()
                .AnyAsync(
                    x => x.Id == request.PricingPlanId &&
                         !x.IsDeleted,
                    cancellationToken);

            if (!pricingPlanExists)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    _localizer["PricingPlanNotFound"]);
            }

            var workspaceTypeExists = await unitOfWork.WorkspaceTypes
                .Query()
                .AnyAsync(
                    x => x.Id == request.WorkspaceTypeId &&
                         !x.IsDeleted,
                    cancellationToken);

            if (!workspaceTypeExists)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    _localizer["WorkspaceTypeNotFound"]);
            }

            var duplicateRule = await unitOfWork.PricingRules
                .Query()
                .AnyAsync(
                    x => x.Id != request.Id &&
                         x.PricingPlanId == request.PricingPlanId &&
                         x.WorkspaceTypeId == request.WorkspaceTypeId &&
                         x.RuleType == request.RuleType &&
                         !x.IsDeleted,
                    cancellationToken);

            if (duplicateRule)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    _localizer["PricingRuleWithSameTypeAlreadyExists"]);
            }

            if (request.IsActive &&
                (request.RuleType == PricingRuleType.RoundUp ||
                 request.RuleType == PricingRuleType.RoundDown))
            {
                var conflictingRuleType =
                    request.RuleType == PricingRuleType.RoundUp
                        ? PricingRuleType.RoundDown
                        : PricingRuleType.RoundUp;

                var conflictingRuleExists = await unitOfWork.PricingRules
                    .Query()
                    .AnyAsync(
                        x => x.Id != request.Id &&
                             x.PricingPlanId == request.PricingPlanId &&
                             x.WorkspaceTypeId == request.WorkspaceTypeId &&
                             x.RuleType == conflictingRuleType &&
                             x.IsActive &&
                             !x.IsDeleted,
                        cancellationToken);

                if (conflictingRuleExists)
                {
                    return Result<bool>.Failure(
                        ResultStatus.Conflict,
                        _localizer[
                            "ActiveConflictingPricingRuleAlreadyExists",
                            conflictingRuleType]);
                }
            }

            pricingRule.PricingPlanId = request.PricingPlanId;
            pricingRule.WorkspaceTypeId = request.WorkspaceTypeId;
            pricingRule.RuleType = request.RuleType;
            pricingRule.Value = request.Value;
            pricingRule.StartDate = request.StartDate;
            pricingRule.EndDate = request.EndDate;
            pricingRule.DayOfWeek = request.DayOfWeek;
            pricingRule.IsActive = request.IsActive;
            pricingRule.UpdatedAt = DateTime.UtcNow;
            pricingRule.UpdatedBy = currentUser.UserId;

            await unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                _localizer["PricingRuleUpdatedSuccessfully"]);
        }
    }
}
