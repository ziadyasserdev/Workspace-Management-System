using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.PricingPlan.Queries.GetPricingPlans
{
    public class GetPricingPlansValidator
        : AbstractValidator<GetPricingPlansQuery>
    {
        public GetPricingPlansValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage(localizer["PageNumberGreaterThanZero"]);

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .LessThanOrEqualTo(100)
                .WithMessage(localizer["PageSizeBetweenOneAndOneHundred"]);

            RuleFor(x => x.Search)
                .MaximumLength(100)
                .When(x => !string.IsNullOrWhiteSpace(x.Search))
                .WithMessage(localizer["SearchMaxLength"]);
        }
    }
}