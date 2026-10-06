
using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Sessions.Services.Commands.ClearServices;

public class ClearServicesValidator : AbstractValidator<ClearServicesCommand>
{
    public ClearServicesValidator(
        IStringLocalizer<SharedResources> localizer)
    {
        RuleFor(x => x.SessionId)
            .GreaterThan(0)
            .WithMessage(localizer["SessionIdGreaterThanZero"]);
    }
}
