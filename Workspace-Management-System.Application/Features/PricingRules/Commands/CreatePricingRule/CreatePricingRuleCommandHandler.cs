using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Resources;
using Workspace_Management_System.Domain.Enums;
using PricingRuleModel = Workspace_Management_System.Domain.Models.PricingRule;

namespace Workspace_Management_System.Application.Features.PricingRule.Commands.CreatePricingRule
{
    public class CreatePricingRuleCommandHandler
        : IRequestHandler<CreatePricingRuleCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUser;
        private readonly IStringLocalizer _localizer;

        public CreatePricingRuleCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser,
            IStringLocalizerFactory factory)
        {
            this.unitOfWork = unitOfWork;
            this.currentUser = currentUser;
            _localizer = factory.Create(typeof(SharedResources));
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
                return Result<int>.Failure(
                    ResultStatus.NotFound,
                    _localizer["WorkspaceTypeNotFound"]);
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
                        x => x.PricingPlanId == request.PricingPlanId &&
                             x.WorkspaceTypeId == request.WorkspaceTypeId &&
                             x.RuleType == conflictingRuleType &&
                             x.IsActive &&
                             !x.IsDeleted,
                        cancellationToken);

                if (conflictingRuleExists)
                {
                    return Result<int>.Failure(
                        ResultStatus.Conflict,
                        _localizer[
                            "ActiveConflictingPricingRuleAlreadyExists",
                            conflictingRuleType]);
                }
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
                _localizer["PricingRuleCreatedSuccessfully"]);
        }
    }
}