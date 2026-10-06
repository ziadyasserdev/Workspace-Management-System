
using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandValidator
        : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(localizer["ProductIdGreaterThanZero"]);

            RuleFor(x => x.ProductCategoryId)
                .GreaterThan(0)
                .WithMessage(localizer["ProductCategoryRequired"]);

            RuleFor(x => x.NameEn)
                .NotEmpty()
                .WithMessage(localizer["ProductEnglishNameRequired"])
                .MaximumLength(150)
                .WithMessage(localizer["ProductEnglishNameMaxLength"]);

            RuleFor(x => x.NameAr)
                .NotEmpty()
                .WithMessage(localizer["ProductArabicNameRequired"])
                .MaximumLength(150)
                .WithMessage(localizer["ProductArabicNameMaxLength"]);

            RuleFor(x => x.DescriptionEn)
                .MaximumLength(500)
                .WithMessage(localizer["EnglishDescriptionMaxLength"]);

            RuleFor(x => x.DescriptionAr)
                .MaximumLength(500)
                .WithMessage(localizer["ArabicDescriptionMaxLength"]);

            RuleFor(x => x.Sku)
                .NotEmpty()
                .WithMessage(localizer["SkuRequired"])
                .MaximumLength(50)
                .WithMessage(localizer["SkuMaxLength"]);

            RuleFor(x => x.SellingPrice)
                .GreaterThanOrEqualTo(0)
                .WithMessage(localizer["SellingPriceGreaterThanOrEqualZero"]);

            RuleFor(x => x.CostPrice)
                .GreaterThanOrEqualTo(0)
                .WithMessage(localizer["CostPriceGreaterThanOrEqualZero"]);
        }
    }
}
