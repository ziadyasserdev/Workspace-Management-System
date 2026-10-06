
using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Sessions.Services.Commands.UpdateService;

public class UpdateServiceValidator : AbstractValidator<UpdateServiceCommand>
{
    public UpdateServiceValidator(
        IStringLocalizer<SharedResources> localizer)
    {
        RuleFor(x => x.SessionId)
            .GreaterThan(0)
            .WithMessage(localizer["SessionIdGreaterThanZero"]);

        RuleFor(x => x.ServiceId)
            .GreaterThan(0)
            .WithMessage(localizer["ServiceIdGreaterThanZero"]);

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage(localizer["QuantityMustBeGreaterThanZero"]);
    }
}
