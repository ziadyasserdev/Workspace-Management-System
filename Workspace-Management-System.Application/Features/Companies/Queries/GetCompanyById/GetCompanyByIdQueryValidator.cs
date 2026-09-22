using FluentValidation;

namespace Workspace_Management_System.Application.Features.Companies.Queries.GetCompanyById
{
    public class GetCompanyByIdQueryValidator
        : AbstractValidator<GetCompanyByIdQuery>
    {
        public GetCompanyByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Company ID must be greater than 0");
        }
    }
}