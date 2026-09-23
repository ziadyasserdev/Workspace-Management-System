using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.PricingRule.Queries;
using Workspace_Management_System.Application.Features.PricingRules.Dtos;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.PricingRule.Queries.GetPricingRules
{
    public class GetPricingRulesQueryHandler
        : IRequestHandler<GetPricingRulesQuery, Result<List<PricingRuleDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetPricingRulesQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<List<PricingRuleDto>>> Handle(
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

            var pricingRules = await query
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

            return Result<List<PricingRuleDto>>.Success(
                pricingRules);
        }
    }
}