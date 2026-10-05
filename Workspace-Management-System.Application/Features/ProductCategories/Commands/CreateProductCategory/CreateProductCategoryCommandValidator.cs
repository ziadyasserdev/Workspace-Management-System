using FluentValidation;

namespace Workspace_Management_System.Application.Features.ProductCategories.Commands.CreateProductCategory
{
    public class CreateProductCategoryCommandValidator
        : AbstractValidator<CreateProductCategoryCommand>
    {
        public CreateProductCategoryCommandValidator()
        {
            RuleFor(x => x.NameEn)
                .NotEmpty()
                .WithMessage("Category English name is required.")
                .MaximumLength(100)
                .WithMessage("Category English name must not exceed 100 characters.");

            RuleFor(x => x.NameAr)
                .NotEmpty()
                .WithMessage("Category Arabic name is required.")
                .MaximumLength(100)
                .WithMessage("Category Arabic name must not exceed 100 characters.");

            RuleFor(x => x.DescriptionEn)
                .MaximumLength(500)
                .WithMessage("English description must not exceed 500 characters.");

            RuleFor(x => x.DescriptionAr)
                .MaximumLength(500)
                .WithMessage("Arabic description must not exceed 500 characters.");
        }
    }
}