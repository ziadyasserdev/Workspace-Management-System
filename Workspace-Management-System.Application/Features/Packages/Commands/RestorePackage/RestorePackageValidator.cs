using FluentValidation;

namespace Workspace_Management_System.Application.Features.Packages.Commands.RestorePackage;

public class RestorePackageValidator
    : AbstractValidator<RestorePackageCommand>
{
    public RestorePackageValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Package ID must be greater than zero.");
    }
}