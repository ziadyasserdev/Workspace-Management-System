using FluentValidation;

namespace Workspace_Management_System.Application.Features.PricingPlan.Commands.CreatePricingPlan
{
    public class CreatePricingPlanValidator
        : AbstractValidator<CreatePricingPlanCommand>
    {
        public CreatePricingPlanValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Pricing plan name is required.")
                .MaximumLength(100)
                .WithMessage("Pricing plan name must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Pricing plan description must not exceed 500 characters.");

            RuleFor(x => x.IsActive)
                .NotNull()
                .WithMessage("Pricing plan active status is required.");
        }
    }
}