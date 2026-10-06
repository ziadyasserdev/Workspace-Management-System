
using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Packages.Commands.RestorePackage;

public class RestorePackageValidator
    : AbstractValidator<RestorePackageCommand>
{
    public RestorePackageValidator(
        IStringLocalizer<SharedResources> localizer)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localizer["PackageIdGreaterThanZero"]);
    }
}
