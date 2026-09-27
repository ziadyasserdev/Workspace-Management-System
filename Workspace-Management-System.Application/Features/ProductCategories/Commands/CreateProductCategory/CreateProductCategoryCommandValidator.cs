using FluentValidation;

namespace Workspace_Management_System.Application.Features.ProductCategories.Commands.CreateProductCategory
{
    public class CreateProductCategoryCommandValidator
        : AbstractValidator<CreateProductCategoryCommand>
    {
        public CreateProductCategoryCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Category name is required.")
                .MaximumLength(100)
                .WithMessage("Category name must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Description must not exceed 500 characters.");
        }
    }
}