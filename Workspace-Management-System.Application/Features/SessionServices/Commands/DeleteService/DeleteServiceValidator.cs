using FluentValidation;

namespace Workspace_Management_System.Application.Features.Sessions.Services.Commands.DeleteService;

public class DeleteServiceValidator : AbstractValidator<DeleteServiceCommand>
{
    public DeleteServiceValidator()
    {
        RuleFor(x => x.SessionId)
            .GreaterThan(0)
            .WithMessage("Session ID must be greater than zero.");

        RuleFor(x => x.ServiceId)
            .GreaterThan(0)
            .WithMessage("Service ID must be greater than zero.");
    }
}