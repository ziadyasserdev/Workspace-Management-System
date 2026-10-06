
using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Sessions.Services.Commands.AddService;

public class AddServiceValidator : AbstractValidator<AddServiceCommand>
{
    public AddServiceValidator(
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
