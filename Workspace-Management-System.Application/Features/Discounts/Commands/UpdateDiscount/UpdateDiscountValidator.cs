
using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Discounts.Commands.UpdateDiscount;

public class UpdateDiscountValidator
    : AbstractValidator<UpdateDiscountCommand>
{
    public UpdateDiscountValidator(
        IStringLocalizer<SharedResources> localizer)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localizer["DiscountIdGreaterThanZero"]);

        RuleFor(x => x.NameEn)
            .NotEmpty()
            .WithMessage(localizer["DiscountEnglishNameRequired"])
            .MaximumLength(100)
            .WithMessage(localizer["DiscountEnglishNameMaxLength"]);

        RuleFor(x => x.NameAr)
            .NotEmpty()
            .WithMessage(localizer["DiscountArabicNameRequired"])
            .MaximumLength(100)
            .WithMessage(localizer["DiscountArabicNameMaxLength"]);

        RuleFor(x => x.DescriptionEn)
            .MaximumLength(500)
            .WithMessage(localizer["EnglishDescriptionMaxLength"]);

        RuleFor(x => x.DescriptionAr)
            .MaximumLength(500)
            .WithMessage(localizer["ArabicDescriptionMaxLength"]);

        RuleFor(x => x.Value)
            .GreaterThan(0)
            .WithMessage(localizer["DiscountValueMustBeGreaterThanZero"]);

        RuleFor(x => x)
            .Must(x =>
                x.Type != DiscountType.Percentage ||
                x.Value <= 100)
            .WithMessage(localizer["PercentageDiscountCannotExceed100"]);

        RuleFor(x => x)
            .Must(x =>
                !x.StartDate.HasValue ||
                !x.EndDate.HasValue ||
                x.EndDate >= x.StartDate)
            .WithMessage(localizer["EndDateCannotBeBeforeStartDate"]);
    }
}
