using FluentValidation;

namespace Workspace_Management_System.Application.Features.Sessions.Services.Commands.ClearServices;

public class ClearServicesValidator : AbstractValidator<ClearServicesCommand>
{
    public ClearServicesValidator()
    {
        RuleFor(x => x.SessionId)
            .GreaterThan(0)
            .WithMessage("Session ID must be greater than zero.");
    }
}