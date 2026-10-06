using FluentValidation;
using Microsoft.Extensions.Localization;
using System.Threading.Channels;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Packages.Queries.GetPackages;

public class GetPackagesValidator
    : AbstractValidator<GetPackagesQuery>
{
    public GetPackagesValidator(
        IStringLocalizer<SharedResources> localizer)
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage(localizer["PageNumberGreaterThanZero"]);

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .LessThanOrEqualTo(100)
            .WithMessage(localizer["PageSizeMustBeBetween1And100"]);

        RuleFor(x => x.Search)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.Search))
            .WithMessage(localizer["SearchCannotExceed100Characters"]);
    }
}
