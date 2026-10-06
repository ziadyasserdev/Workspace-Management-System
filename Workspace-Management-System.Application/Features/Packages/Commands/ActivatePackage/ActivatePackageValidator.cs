using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Packages.Commands.ActivatePackage
{
    public class ActivatePackageValidator
        : AbstractValidator<ActivatePackageCommand>
    {
        public ActivatePackageValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(localizer["PackageIdGreaterThanZero"]);
        }
    }
}
