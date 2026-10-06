using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.PricingPlan.Queries.GetPricingPlanById
{
    public class GetPricingPlanByIdValidator
        : AbstractValidator<GetPricingPlanByIdQuery>
    {
        public GetPricingPlanByIdValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(localizer["PricingPlanIdGreaterThanZero"]);
        }
    }
}