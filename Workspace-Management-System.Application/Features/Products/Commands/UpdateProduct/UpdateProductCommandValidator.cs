using FluentValidation;

namespace Workspace_Management_System.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandValidator
        : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

            RuleFor(x => x.ProductCategoryId)
                .GreaterThan(0)
                .WithMessage("Product category is required.");

            RuleFor(x => x.NameEn)
                .NotEmpty()
                .WithMessage("Product English name is required.")
                .MaximumLength(150)
                .WithMessage("Product English name must not exceed 150 characters.");

            RuleFor(x => x.NameAr)
                .NotEmpty()
                .WithMessage("Product Arabic name is required.")
                .MaximumLength(150)
                .WithMessage("Product Arabic name must not exceed 150 characters.");

            RuleFor(x => x.DescriptionEn)
                .MaximumLength(500)
                .WithMessage("English description must not exceed 500 characters.");

            RuleFor(x => x.DescriptionAr)
                .MaximumLength(500)
                .WithMessage("Arabic description must not exceed 500 characters.");

            RuleFor(x => x.Sku)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.SellingPrice)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.CostPrice)
                .GreaterThanOrEqualTo(0);
        }
    }
}