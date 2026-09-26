using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;

namespace Workspace_Management_System.Application.Features.PricingPlan.Queries.GetPricingPlanRules;

public class GetPricingPlanRulesQueryHandler
    : IRequestHandler<
        GetPricingPlanRulesQuery,
        Result<List<PricingPlanRuleDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPricingPlanRulesQueryHandler(
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<PricingPlanRuleDto>>> Handle(
        GetPricingPlanRulesQuery request,
        CancellationToken cancellationToken)
    {
        var pricingPlanExists = await _unitOfWork.PricingPlans
            .Query()
            .AnyAsync(
                x => x.Id == request.Id &&
                     !x.IsDeleted,
                cancellationToken);

        if (!pricingPlanExists)
        {
            return Result<List<PricingPlanRuleDto>>.Failure(
                ResultStatus.NotFound,
                "Pricing plan not found.");
        }

        var rules = await _unitOfWork.PricingRules
            .Query()
            .Where(x =>
                x.PricingPlanId == request.Id &&
                !x.IsDeleted)
            .OrderBy(x => x.WorkspaceTypeId)
            .ThenBy(x => x.RuleType)
            .Select(x => new PricingPlanRuleDto
            {
                Id = x.Id,
                PricingPlanId = x.PricingPlanId,
                WorkspaceTypeId = x.WorkspaceTypeId,
                RuleType = x.RuleType.ToString(),
                Value = x.Value,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                DayOfWeek = x.DayOfWeek.HasValue
                    ? x.DayOfWeek.Value.ToString()
                    : null,
                IsActive = x.IsActive
            })
            .ToListAsync(cancellationToken);

        return Result<List<PricingPlanRuleDto>>.Success(rules);
    }
}