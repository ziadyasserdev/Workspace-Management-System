using FluentValidation;

namespace Workspace_Management_System.Application.Features.ProductCategories.Queries.GetProductCategoryById
{
    public class GetProductCategoryByIdQueryValidator
        : AbstractValidator<GetProductCategoryByIdQuery>
    {
        public GetProductCategoryByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Invalid product category ID.");
        }
    }
}