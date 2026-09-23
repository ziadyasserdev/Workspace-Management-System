using FluentValidation;

namespace Workspace_Management_System.Application.Features.PricingRule.Queries.GetPricingRules
{
    public class GetPricingRulesValidator
        : AbstractValidator<GetPricingRulesQuery>
    {
        public GetPricingRulesValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than zero.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .LessThanOrEqualTo(100)
                .WithMessage("Page size must be between 1 and 100.");

            RuleFor(x => x.PricingPlanId)
                .GreaterThan(0)
                .When(x => x.PricingPlanId.HasValue)
                .WithMessage("Pricing plan ID must be greater than zero.");

            RuleFor(x => x.WorkspaceTypeId)
                .GreaterThan(0)
                .When(x => x.WorkspaceTypeId.HasValue)
                .WithMessage("Workspace type ID must be greater than zero.");
        }
    }
}