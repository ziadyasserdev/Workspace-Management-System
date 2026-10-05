using FluentValidation;

namespace Workspace_Management_System.Application.Features.PricingPlan.Commands.CreatePricingPlan
{
    public class CreatePricingPlanValidator
        : AbstractValidator<CreatePricingPlanCommand>
    {
        public CreatePricingPlanValidator()
        {
            RuleFor(x => x.NameEn)
                .NotEmpty()
                .WithMessage("Pricing plan English name is required.")
                .MaximumLength(100)
                .WithMessage("Pricing plan English name must not exceed 100 characters.");

            RuleFor(x => x.NameAr)
                .NotEmpty()
                .WithMessage("Pricing plan Arabic name is required.")
                .MaximumLength(100)
                .WithMessage("Pricing plan Arabic name must not exceed 100 characters.");

            RuleFor(x => x.DescriptionEn)
                .MaximumLength(500)
                .WithMessage("English description must not exceed 500 characters.");

            RuleFor(x => x.DescriptionAr)
                .MaximumLength(500)
                .WithMessage("Arabic description must not exceed 500 characters.");
        }
    }
}