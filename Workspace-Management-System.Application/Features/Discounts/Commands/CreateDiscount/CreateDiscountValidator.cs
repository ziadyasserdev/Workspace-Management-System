
using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Discounts.Commands.CreateDiscount;

public class CreateDiscountValidator
    : AbstractValidator<CreateDiscountCommand>
{
    public CreateDiscountValidator(
        IStringLocalizer<SharedResources> localizer)
    {
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
                x.Type != Domain.Enums.DiscountType.Percentage ||
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
