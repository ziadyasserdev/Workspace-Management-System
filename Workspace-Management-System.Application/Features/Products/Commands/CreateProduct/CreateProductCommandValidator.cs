using FluentValidation;

namespace Workspace_Management_System.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandValidator
        : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
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
                .WithMessage("SKU is required.")
                .MaximumLength(50)
                .WithMessage("SKU must not exceed 50 characters.");

            RuleFor(x => x.SellingPrice)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Selling price must be greater than or equal to 0.");

            RuleFor(x => x.CostPrice)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Cost price must be greater than or equal to 0.");
        }
    }
}