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

            RuleFor(x => x.EnglishName)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.Description)
                .MaximumLength(500);

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