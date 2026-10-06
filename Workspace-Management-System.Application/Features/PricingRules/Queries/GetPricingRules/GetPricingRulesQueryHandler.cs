using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.PaginatedResults;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.PricingRules.Dtos;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.PricingRule.Queries.GetPricingRules
{
    public class GetPricingRulesQueryHandler
        : IRequestHandler<
            GetPricingRulesQuery,
            Result<PaginatedResult<PricingRuleDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IStringLocalizer _localizer;

        public GetPricingRulesQueryHandler(
            IUnitOfWork unitOfWork,
            IStringLocalizerFactory factory)
        {
            this.unitOfWork = unitOfWork;
            _localizer = factory.Create(typeof(SharedResources));
        }

        public async Task<Result<PaginatedResult<PricingRuleDto>>> Handle(
            GetPricingRulesQuery request,
            CancellationToken cancellationToken)
        {
            var query = unitOfWork.PricingRules
                .Query()
                .AsNoTracking()
                .Where(x => !x.IsDeleted);

            if (request.PricingPlanId.HasValue)
            {
                query = query.Where(x =>
                    x.PricingPlanId == request.PricingPlanId.Value);
            }

            if (request.WorkspaceTypeId.HasValue)
            {
                query = query.Where(x =>
                    x.WorkspaceTypeId == request.WorkspaceTypeId.Value);
            }

            if (request.RuleType.HasValue)
            {
                query = query.Where(x =>
                    x.RuleType == request.RuleType.Value);
            }

            if (request.IsActive.HasValue)
            {
                query = query.Where(x =>
                    x.IsActive == request.IsActive.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var pricingRules = await query
                .OrderBy(x => x.PricingPlanId)
                .ThenBy(x => x.WorkspaceTypeId)
                .ThenBy(x => x.RuleType)
                .Select(x => new
                {
                    x.Id,
                    x.PricingPlanId,
                    x.WorkspaceTypeId,
                    x.RuleType,
                    x.Value,
                    x.StartDate,
                    x.EndDate,
                    x.DayOfWeek,
                    x.IsActive
                })
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var resultItems = pricingRules
                .Select(x => new PricingRuleDto
                {
                    Id = x.Id,
                    PricingPlanId = x.PricingPlanId,
                    WorkspaceTypeId = x.WorkspaceTypeId,
                    RuleType = _localizer[
                        $"PricingRuleType_{x.RuleType}"],
                    Value = x.Value,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    DayOfWeek = x.DayOfWeek.HasValue
                        ? _localizer[
                            $"DayOfWeek_{x.DayOfWeek.Value}"]
                        : null,
                    IsActive = x.IsActive
                })
                .ToList();

            var result = new PaginatedResult<PricingRuleDto>(
                resultItems,
                request.PageNumber,
                request.PageSize,
                totalCount);

            return Result<PaginatedResult<PricingRuleDto>>.Success(
                result,
                _localizer["PricingRulesRetrievedSuccessfully"]);
        }
    }
}

