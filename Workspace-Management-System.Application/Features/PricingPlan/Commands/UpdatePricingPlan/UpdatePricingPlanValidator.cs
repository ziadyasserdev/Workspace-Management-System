using FluentValidation;

namespace Workspace_Management_System.Application.Features.PricingPlan.Commands.UpdatePricingPlan
{
    public class UpdatePricingPlanValidator
        : AbstractValidator<UpdatePricingPlanCommand>
    {
        public UpdatePricingPlanValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Pricing plan ID must be greater than zero.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Pricing plan name is required.")
                .MaximumLength(100)
                .WithMessage("Pricing plan name must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Pricing plan description must not exceed 500 characters.");
        }
    }
}