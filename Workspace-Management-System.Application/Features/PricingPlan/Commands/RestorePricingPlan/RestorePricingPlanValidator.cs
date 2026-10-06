using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.PricingPlan.Commands.RestorePricingPlan
{
    public class RestorePricingPlanValidator
        : AbstractValidator<RestorePricingPlanCommand>
    {
        public RestorePricingPlanValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(localizer["PricingPlanIdGreaterThanZero"]);
        }
    }
}