using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.PaginatedResults;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.PricingRules.Dtos;

namespace Workspace_Management_System.Application.Features.PricingRule.Queries.GetPricingRules
{
    public class GetPricingRulesQueryHandler
        : IRequestHandler<
            GetPricingRulesQuery,
            Result<PaginatedResult<PricingRuleDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetPricingRulesQueryHandler(
            IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<PaginatedResult<PricingRuleDto>>> Handle(
            GetPricingRulesQuery request,
            CancellationToken cancellationToken)
        {
            var query = unitOfWork.PricingRules
                .Query()
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

            var totalCount = await query
                .CountAsync(cancellationToken);

            var pricingRules = await query
                .OrderBy(x => x.PricingPlanId)
                .ThenBy(x => x.WorkspaceTypeId)
                .ThenBy(x => x.RuleType)
                .Select(x => new PricingRuleDto
                {
                    Id = x.Id,
                    PricingPlanId = x.PricingPlanId,
                    WorkspaceTypeId = x.WorkspaceTypeId,
                    RuleType = x.RuleType,
                    Value = x.Value,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    DayOfWeek = x.DayOfWeek,
                    IsActive = x.IsActive
                })
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var result = new PaginatedResult<PricingRuleDto>(
                pricingRules,
                request.PageNumber,
                request.PageSize,
                totalCount);

            return Result<PaginatedResult<PricingRuleDto>>.Success(
                result);
        }
    }
}