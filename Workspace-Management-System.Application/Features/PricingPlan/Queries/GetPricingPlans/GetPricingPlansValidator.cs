using FluentValidation;

namespace Workspace_Management_System.Application.Features.PricingPlan.Queries.GetPricingPlans
{
    public class GetPricingPlansValidator
        : AbstractValidator<GetPricingPlansQuery>
    {
        public GetPricingPlansValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than zero.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .LessThanOrEqualTo(100)
                .WithMessage("Page size must be between 1 and 100.");

            RuleFor(x => x.Search)
                .MaximumLength(100)
                .WithMessage("Search must not exceed 100 characters.");
        }
    }
}