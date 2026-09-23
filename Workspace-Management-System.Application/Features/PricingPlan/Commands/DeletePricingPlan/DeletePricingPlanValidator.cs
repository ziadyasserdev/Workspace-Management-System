using FluentValidation;

namespace Workspace_Management_System.Application.Features.PricingPlan.Commands.DeletePricingPlan
{
    public class DeletePricingPlanValidator
        : AbstractValidator<DeletePricingPlanCommand>
    {
        public DeletePricingPlanValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Pricing plan ID must be greater than zero.");
        }
    }
}