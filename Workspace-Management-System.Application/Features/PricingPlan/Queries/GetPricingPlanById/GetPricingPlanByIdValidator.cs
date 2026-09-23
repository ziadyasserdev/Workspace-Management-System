using FluentValidation;

namespace Workspace_Management_System.Application.Features.PricingPlan.Queries.GetPricingPlanById
{
    public class GetPricingPlanByIdValidator
        : AbstractValidator<GetPricingPlanByIdQuery>
    {
        public GetPricingPlanByIdValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Pricing plan ID must be greater than zero.");
        }
    }
}