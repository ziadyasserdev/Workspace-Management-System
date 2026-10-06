using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.PricingRule.Queries.GetPricingRuleById
{
    public class GetPricingRuleByIdValidator
        : AbstractValidator<GetPricingRuleByIdQuery>
    {
        public GetPricingRuleByIdValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(localizer["PricingRuleIdGreaterThanZero"]);
        }
    }
}