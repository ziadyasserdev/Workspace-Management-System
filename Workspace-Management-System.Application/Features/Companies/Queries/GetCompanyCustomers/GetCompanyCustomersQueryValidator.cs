using FluentValidation;

namespace Workspace_Management_System.Application.Features.Companies.Queries.GetCompanyCustomers
{
    public class GetCompanyCustomersQueryValidator
        : AbstractValidator<GetCompanyCustomersQuery>
    {
        public GetCompanyCustomersQueryValidator()
        {
            RuleFor(x => x.CompanyId)
                .GreaterThan(0)
                .WithMessage("Company ID must be greater than 0");

            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than 0");

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .LessThanOrEqualTo(100)
                .WithMessage("Page size must be between 1 and 100");
        }
    }
}