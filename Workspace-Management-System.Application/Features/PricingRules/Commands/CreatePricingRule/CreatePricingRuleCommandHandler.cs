using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using PricingRuleModel = Workspace_Management_System.Domain.Models.PricingRule;
namespace Workspace_Management_System.Application.Features.PricingRule.Commands.CreatePricingRule
{
    public class CreatePricingRuleCommandHandler
        : IRequestHandler<CreatePricingRuleCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUser;

        public CreatePricingRuleCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            this.unitOfWork = unitOfWork;
            this.currentUser = currentUser;
        }

        public async Task<Result<int>> Handle(
            CreatePricingRuleCommand request,
            CancellationToken cancellationToken)
        {
            var pricingPlanExists = await unitOfWork.PricingPlans
                .Query()
                .AnyAsync(
                    x => x.Id == request.PricingPlanId &&
                         !x.IsDeleted,
                    cancellationToken);

            if (!pricingPlanExists)
            {
                return Result<int>.Failure(
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
                return Result<int>.Failure(
                    ResultStatus.NotFound,
                    "Workspace type not found.");
            }

            var duplicateRule = await unitOfWork.PricingRules
                .Query()
                .AnyAsync(
                    x => x.PricingPlanId == request.PricingPlanId &&
                         x.WorkspaceTypeId == request.WorkspaceTypeId &&
                         x.RuleType == request.RuleType &&
                         !x.IsDeleted,
                    cancellationToken);

            if (duplicateRule)
            {
                return Result<int>.Failure(
                    ResultStatus.Conflict,
                    "A pricing rule with the same type already exists for this pricing plan and workspace type.");
            }

            var pricingRule = new PricingRuleModel
            {
                PricingPlanId = request.PricingPlanId,
                WorkspaceTypeId = request.WorkspaceTypeId,
                RuleType = request.RuleType,
                Value = request.Value,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                DayOfWeek = request.DayOfWeek,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUser.UserId
            };

            await unitOfWork.PricingRules.AddAsync(pricingRule);
            await unitOfWork.SaveAsync();

            return Result<int>.Success(
                pricingRule.Id,
                "Pricing rule created successfully.");
        }
    }
}