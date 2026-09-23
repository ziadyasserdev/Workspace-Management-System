using FluentValidation;

namespace Workspace_Management_System.Application.Features.PricingPlan.Commands.RestorePricingPlan
{
    public class RestorePricingPlanValidator
        : AbstractValidator<RestorePricingPlanCommand>
    {
        public RestorePricingPlanValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Pricing plan ID must be greater than zero.");
        }
    }
}