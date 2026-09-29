using MediatR;

namespace Workspace_Management_System.Application.Features.Sessions.Services.Commands.ClearServices;

public class ClearServicesCommand : IRequest<bool>
{
    public int SessionId { get; set; }
}