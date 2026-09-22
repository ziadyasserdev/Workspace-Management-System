using FluentValidation;

namespace Workspace_Management_System.Application.Features.Companies.Queries.SearchCompanies
{
    public class SearchCompaniesQueryValidator
        : AbstractValidator<SearchCompaniesQuery>
    {
        public SearchCompaniesQueryValidator()
        {
            RuleFor(x => x.SearchTerm)
                .NotEmpty()
                .WithMessage("Search term is required")
                .MaximumLength(100)
                .WithMessage("Search term must not exceed 100 characters");

            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than 0");

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .WithMessage("Page size must be greater than 0")
                .LessThanOrEqualTo(100)
                .WithMessage("Page size must not exceed 100");
        }
    }
}