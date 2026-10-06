using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Packages.Queries.GetPackageById;

public class GetPackageByIdValidator
    : AbstractValidator<GetPackageByIdQuery>
{
    public GetPackageByIdValidator(
        IStringLocalizer<SharedResources> localizer)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localizer["PackageIdGreaterThanZero"]);
    }
}
