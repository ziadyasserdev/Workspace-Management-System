using FluentValidation;

namespace Workspace_Management_System.Application.Features.PricingRule.Commands.DeletePricingRule
{
    public class DeletePricingRuleValidator
        : AbstractValidator<DeletePricingRuleCommand>
    {
        public DeletePricingRuleValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Pricing rule ID must be greater than zero.");
        }
    }
}