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

            RuleFor(x => x.EnglishName)
                .NotEmpty()
                .WithMessage("Product English name is required.")
                .MaximumLength(150)
                .WithMessage("Product English name must not exceed 150 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Description must not exceed 500 characters.");

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