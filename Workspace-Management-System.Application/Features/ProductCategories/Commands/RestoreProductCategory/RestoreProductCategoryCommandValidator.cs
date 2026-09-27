using FluentValidation;

namespace Workspace_Management_System.Application.Features.ProductCategories.Commands.RestoreProductCategory
{
    public class RestoreProductCategoryCommandValidator
        : AbstractValidator<RestoreProductCategoryCommand>
    {
        public RestoreProductCategoryCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Invalid category ID.");
        }
    }
}