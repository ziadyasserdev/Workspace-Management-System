using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.PricingRule.Commands.UpdatePricingRule
{
    public class UpdatePricingRuleValidator
        : AbstractValidator<UpdatePricingRuleCommand>
    {
        public UpdatePricingRuleValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(localizer["PricingRuleIdGreaterThanZero"]);

            RuleFor(x => x.PricingPlanId)
                .GreaterThan(0)
                .WithMessage(localizer["PricingPlanIdGreaterThanZero"]);

            RuleFor(x => x.WorkspaceTypeId)
                .GreaterThan(0)
                .WithMessage(localizer["WorkspaceTypeIdGreaterThanZero"]);

            RuleFor(x => x.Value)
                .GreaterThanOrEqualTo(0)
                .When(x => x.Value.HasValue)
                .WithMessage(localizer["PricingRuleValueGreaterThanOrEqualZero"]);

            RuleFor(x => x.EndDate)
                .GreaterThanOrEqualTo(x => x.StartDate)
                .When(x => x.StartDate.HasValue && x.EndDate.HasValue)
                .WithMessage(localizer["PricingRuleEndDateGreaterThanOrEqualStartDate"]);
        }
    }
}