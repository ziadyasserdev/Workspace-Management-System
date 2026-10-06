using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.PricingRule.Commands.DeletePricingRule
{
    public class DeletePricingRuleValidator
        : AbstractValidator<DeletePricingRuleCommand>
    {
        public DeletePricingRuleValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(localizer["PricingRuleIdGreaterThanZero"]);
        }
    }
}