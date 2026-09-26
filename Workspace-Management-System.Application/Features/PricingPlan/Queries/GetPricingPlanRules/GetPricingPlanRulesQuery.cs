using MediatR;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.PricingPlan.Queries.GetPricingPlanRules;

public class GetPricingPlanRulesQuery
    : IRequest<Result<List<PricingPlanRuleDto>>>
{
    public int Id { get; set; }
}