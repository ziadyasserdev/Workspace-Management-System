using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.PricingPlan.Queries.GetPricingPlanRules;

public class GetPricingPlanRulesQueryValidator
    : AbstractValidator<GetPricingPlanRulesQuery>
{
    public GetPricingPlanRulesQueryValidator(
        IStringLocalizer<SharedResources> localizer)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localizer["PricingPlanIdGreaterThanZero"]);
    }
}