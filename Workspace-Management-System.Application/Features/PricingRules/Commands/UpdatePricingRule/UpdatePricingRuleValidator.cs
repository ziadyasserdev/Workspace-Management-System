using FluentValidation;

namespace Workspace_Management_System.Application.Features.PricingRule.Commands.UpdatePricingRule
{
    public class UpdatePricingRuleValidator
        : AbstractValidator<UpdatePricingRuleCommand>
    {
        public UpdatePricingRuleValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Pricing rule ID must be greater than zero.");

            RuleFor(x => x.PricingPlanId)
                .GreaterThan(0)
                .WithMessage("Pricing plan ID must be greater than zero.");

            RuleFor(x => x.WorkspaceTypeId)
                .GreaterThan(0)
                .WithMessage("Workspace type ID must be greater than zero.");

            RuleFor(x => x.Value)
                .GreaterThanOrEqualTo(0)
                .When(x => x.Value.HasValue)
                .WithMessage("Pricing rule value must be greater than or equal to zero.");

            RuleFor(x => x.EndDate)
                .GreaterThanOrEqualTo(x => x.StartDate)
                .When(x => x.StartDate.HasValue && x.EndDate.HasValue)
                .WithMessage("End date must be greater than or equal to start date.");
        }
    }
}