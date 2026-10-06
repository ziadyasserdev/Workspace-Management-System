using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.ProductCategories.Commands.CreateProductCategory;

public class CreateProductCategoryCommandValidator
    : AbstractValidator<CreateProductCategoryCommand>
{
    public CreateProductCategoryCommandValidator(
        IStringLocalizer<SharedResources> localizer)
    {
        RuleFor(x => x.NameEn)
            .NotEmpty()
            .WithMessage(localizer["ProductCategoryEnglishNameRequired"])
            .MaximumLength(100)
            .WithMessage(localizer["ProductCategoryEnglishNameMaxLength"]);

        RuleFor(x => x.NameAr)
            .NotEmpty()
            .WithMessage(localizer["ProductCategoryArabicNameRequired"])
            .MaximumLength(100)
            .WithMessage(localizer["ProductCategoryArabicNameMaxLength"]);

        RuleFor(x => x.DescriptionEn)
            .MaximumLength(500)
            .WithMessage(localizer["ProductCategoryEnglishDescriptionMaxLength"]);

        RuleFor(x => x.DescriptionAr)
            .MaximumLength(500)
            .WithMessage(localizer["ProductCategoryArabicDescriptionMaxLength"]);
    }
}