using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;

namespace Workspace_Management_System.Application.Features.PricingRule.Commands.UpdatePricingRule
{
    public class UpdatePricingRuleCommandHandler
        : IRequestHandler<UpdatePricingRuleCommand, Result<bool>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUser;

        public UpdatePricingRuleCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            this.unitOfWork = unitOfWork;
            this.currentUser = currentUser;
        }

        public async Task<Result<bool>> Handle(
            UpdatePricingRuleCommand request,
            CancellationToken cancellationToken)
        {
            var pricingRule = await unitOfWork.PricingRules
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.IsDeleted,
                    cancellationToken);

            if (pricingRule == null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    "Pricing rule not found.");
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
                    "Pricing plan not found.");
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
                    "Workspace type not found.");
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
                    "A pricing rule with the same type already exists for this pricing plan and workspace type.");
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
                "Pricing rule updated successfully.");
        }
    }
}