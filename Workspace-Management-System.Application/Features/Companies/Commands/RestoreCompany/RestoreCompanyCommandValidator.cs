using FluentValidation;

namespace Workspace_Management_System.Application.Features.Companies.Commands.RestoreCompany
{
    public class RestoreCompanyCommandValidator
        : AbstractValidator<RestoreCompanyCommand>
    {
        public RestoreCompanyCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Company ID must be greater than 0");
        }
    }
}