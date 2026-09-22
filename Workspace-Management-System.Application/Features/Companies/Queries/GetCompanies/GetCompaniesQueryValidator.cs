using FluentValidation;

namespace Workspace_Management_System.Application.Features.Companies.Queries.GetCompanies
{
    public class GetCompaniesQueryValidator
        : AbstractValidator<GetCompaniesQuery>
    {
        public GetCompaniesQueryValidator()
        {
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