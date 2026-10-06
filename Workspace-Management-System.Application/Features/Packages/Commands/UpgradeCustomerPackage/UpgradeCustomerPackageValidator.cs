using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;
using Workspace_Management_System.Domain.Models;
using static System.Net.Mime.MediaTypeNames;

namespace Workspace_Management_System.Application.Features.Packages.Commands.UpgradeCustomerPackage;

public class UpgradeCustomerPackageValidator
    : AbstractValidator<UpgradeCustomerPackageCommand>
{
    public UpgradeCustomerPackageValidator(
        IStringLocalizer<SharedResources> localizer)
    {
        RuleFor(x => x.CustomerId)
            .GreaterThan(0)
            .WithMessage(localizer["CustomerIdGreaterThanZero"]);

        RuleFor(x => x.NewPackageId)
            .GreaterThan(0)
            .WithMessage(localizer["NewPackageIdGreaterThanZero"]);
    }
}
