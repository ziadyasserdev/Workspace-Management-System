using FluentValidation;

namespace Workspace_Management_System.Application.Features.Packages.Commands.DeactivatePackage;

public class DeactivatePackageValidator
    : AbstractValidator<DeactivatePackageCommand>
{
    public DeactivatePackageValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Package ID must be greater than zero.");
    }
}