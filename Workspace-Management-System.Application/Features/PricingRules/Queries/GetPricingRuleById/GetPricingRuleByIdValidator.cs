using FluentValidation;

namespace Workspace_Management_System.Application.Features.PricingRule.Queries.GetPricingRuleById
{
    public class GetPricingRuleByIdValidator
        : AbstractValidator<GetPricingRuleByIdQuery>
    {
        public GetPricingRuleByIdValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Pricing rule ID must be greater than zero.");
        }
    }
}