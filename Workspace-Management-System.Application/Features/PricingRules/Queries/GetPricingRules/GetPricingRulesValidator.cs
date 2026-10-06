using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.PricingRule.Queries.GetPricingRules
{
    public class GetPricingRulesValidator
        : AbstractValidator<GetPricingRulesQuery>
    {
        public GetPricingRulesValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage(localizer["PageNumberGreaterThanZero"]);

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .LessThanOrEqualTo(100)
                .WithMessage(localizer["PageSizeBetweenOneAndOneHundred"]);

            RuleFor(x => x.PricingPlanId)
                .GreaterThan(0)
                .When(x => x.PricingPlanId.HasValue)
                .WithMessage(localizer["PricingPlanIdGreaterThanZero"]);

            RuleFor(x => x.WorkspaceTypeId)
                .GreaterThan(0)
                .When(x => x.WorkspaceTypeId.HasValue)
                .WithMessage(localizer["WorkspaceTypeIdGreaterThanZero"]);
        }
    }
}