using FluentValidation;

namespace Workspace_Management_System.Application.Features.Packages.Commands.ActivatePackage;

public class ActivatePackageValidator
    : AbstractValidator<ActivatePackageCommand>
{
    public ActivatePackageValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Package ID must be greater than zero.");
    }
}