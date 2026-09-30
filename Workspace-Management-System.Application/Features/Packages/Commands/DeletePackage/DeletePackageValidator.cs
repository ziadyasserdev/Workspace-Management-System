using FluentValidation;

namespace Workspace_Management_System.Application.Features.Packages.Commands.DeletePackage;

public class DeletePackageValidator
    : AbstractValidator<DeletePackageCommand>
{
    public DeletePackageValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Package ID must be greater than zero.");
    }
}