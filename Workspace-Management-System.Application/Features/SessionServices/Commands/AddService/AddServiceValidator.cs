using FluentValidation;

namespace Workspace_Management_System.Application.Features.Sessions.Services.Commands.AddService;

public class AddServiceValidator : AbstractValidator<AddServiceCommand>
{
    public AddServiceValidator()
    {
        RuleFor(x => x.SessionId)
            .GreaterThan(0)
            .WithMessage("SessionId must be greater than zero.");

        RuleFor(x => x.ServiceId)
            .GreaterThan(0)
            .WithMessage("ServiceId must be greater than zero.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero.");
    }
}

