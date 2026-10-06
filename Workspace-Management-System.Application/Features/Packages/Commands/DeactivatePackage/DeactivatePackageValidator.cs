using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Packages.Commands.DeactivatePackage;

public class DeactivatePackageValidator
    : AbstractValidator<DeactivatePackageCommand>
{
    public DeactivatePackageValidator(
        IStringLocalizer<SharedResources> localizer)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localizer["PackageIdGreaterThanZero"]);
    }
}
