using FluentValidation;

namespace Workspace_Management_System.Application.Features.Companies.Commands.ChangeCompanyStatus
{
    public class ChangeCompanyStatusCommandValidator
        : AbstractValidator<ChangeCompanyStatusCommand>
    {
        public ChangeCompanyStatusCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Company ID must be greater than 0");
        }
    }
}