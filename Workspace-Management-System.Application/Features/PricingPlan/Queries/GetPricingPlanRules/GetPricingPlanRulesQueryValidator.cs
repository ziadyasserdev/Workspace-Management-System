using FluentValidation;

namespace Workspace_Management_System.Application.Features.PricingPlan.Queries.GetPricingPlanRules;

public class GetPricingPlanRulesQueryValidator
    : AbstractValidator<GetPricingPlanRulesQuery>
{
    public GetPricingPlanRulesQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Pricing plan ID must be greater than zero.");
    }
}