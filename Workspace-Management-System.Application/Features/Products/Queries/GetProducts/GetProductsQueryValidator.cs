using FluentValidation;

namespace Workspace_Management_System.Application.Features.Products.Queries.GetProducts
{
    public class GetProductsQueryValidator
        : AbstractValidator<GetProductsQuery>
    {
        public GetProductsQueryValidator()
        {
            RuleFor(x => x.Search)
                .MaximumLength(150)
                .WithMessage("Search must not exceed 150 characters.");

            RuleFor(x => x.ProductCategoryId)
                .GreaterThan(0)
                .When(x => x.ProductCategoryId.HasValue)
                .WithMessage("Product category ID must be greater than 0.");

            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than 0.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("Page size must be between 1 and 100.");
        }
    }
}