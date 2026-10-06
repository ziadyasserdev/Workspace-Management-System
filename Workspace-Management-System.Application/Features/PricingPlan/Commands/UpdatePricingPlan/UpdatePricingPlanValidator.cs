using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.PricingPlan.Commands.UpdatePricingPlan
{
    public class UpdatePricingPlanValidator
        : AbstractValidator<UpdatePricingPlanCommand>
    {
        public UpdatePricingPlanValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(localizer["PricingPlanIdGreaterThanZero"]);

            RuleFor(x => x.NameEn)
                .NotEmpty()
                .WithMessage(localizer["PricingPlanEnglishNameRequired"])
                .MaximumLength(100)
                .WithMessage(localizer["PricingPlanEnglishNameMaxLength"]);

            RuleFor(x => x.NameAr)
                .NotEmpty()
                .WithMessage(localizer["PricingPlanArabicNameRequired"])
                .MaximumLength(100)
                .WithMessage(localizer["PricingPlanArabicNameMaxLength"]);

            RuleFor(x => x.DescriptionEn)
                .MaximumLength(500)
                .WithMessage(localizer["EnglishDescriptionMaxLength"]);

            RuleFor(x => x.DescriptionAr)
                .MaximumLength(500)
                .WithMessage(localizer["ArabicDescriptionMaxLength"]);
        }
    }
}