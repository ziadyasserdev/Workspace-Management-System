using FluentValidation;

namespace Workspace_Management_System.Application.Features.PricingRule.Commands.RestorePricingRule
{
    public class RestorePricingRuleValidator
        : AbstractValidator<RestorePricingRuleCommand>
    {
        public RestorePricingRuleValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Pricing rule ID must be greater than zero.");
        }
    }
}