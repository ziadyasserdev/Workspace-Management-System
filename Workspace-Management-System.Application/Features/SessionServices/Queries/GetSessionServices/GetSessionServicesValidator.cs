
using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Sessions.Services.Queries.GetSessionServices;

public class GetSessionServicesValidator
    : AbstractValidator<GetSessionServicesQuery>
{
    public GetSessionServicesValidator(
        IStringLocalizer<SharedResources> localizer)
    {
        RuleFor(x => x.SessionId)
            .GreaterThan(0)
            .WithMessage(localizer["SessionIdGreaterThanZero"]);

        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage(localizer["PageNumberGreaterThanZero"]);

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .LessThanOrEqualTo(100)
            .WithMessage(localizer["PageSizeBetweenOneAndOneHundred"]);

        RuleFor(x => x.Search)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.Search))
            .WithMessage(localizer["SearchMaxLength"]);
    }
}
