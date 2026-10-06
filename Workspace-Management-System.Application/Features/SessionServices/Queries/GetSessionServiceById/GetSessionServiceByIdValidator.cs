
using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Sessions.Services.Queries.GetSessionServiceById;

public class GetSessionServiceByIdValidator
    : AbstractValidator<GetSessionServiceByIdQuery>
{
    public GetSessionServiceByIdValidator(
        IStringLocalizer<SharedResources> localizer)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(localizer["SessionServiceIdGreaterThanZero"]);
    }
}
