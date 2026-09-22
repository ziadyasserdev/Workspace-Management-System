using FluentValidation;

namespace Workspace_Management_System.Application.Features.Companies.Commands.DeleteCompany
{
    public class DeleteCompanyCommandValidator
        : AbstractValidator<DeleteCompanyCommand>
    {
        public DeleteCompanyCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Company ID must be greater than 0");
        }
    }
}