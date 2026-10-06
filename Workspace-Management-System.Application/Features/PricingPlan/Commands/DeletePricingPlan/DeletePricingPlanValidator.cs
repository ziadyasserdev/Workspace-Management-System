using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.PricingPlan.Commands.DeletePricingPlan
{
    public class DeletePricingPlanValidator
        : AbstractValidator<DeletePricingPlanCommand>
    {
        public DeletePricingPlanValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(localizer["PricingPlanIdGreaterThanZero"]);
        }
    }
}