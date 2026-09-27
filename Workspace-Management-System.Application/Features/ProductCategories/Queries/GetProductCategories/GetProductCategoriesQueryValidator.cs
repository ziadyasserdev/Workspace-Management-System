using FluentValidation;

namespace Workspace_Management_System.Application.Features.ProductCategories.Queries.GetProductCategories
{
    public class GetProductCategoriesQueryValidator
        : AbstractValidator<GetProductCategoriesQuery>
    {
        public GetProductCategoriesQueryValidator()
        {
            RuleFor(x => x.Search)
                .MaximumLength(100)
                .WithMessage("Search must not exceed 100 characters.");

            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than 0.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("Page size must be between 1 and 100.");
        }
    }
}