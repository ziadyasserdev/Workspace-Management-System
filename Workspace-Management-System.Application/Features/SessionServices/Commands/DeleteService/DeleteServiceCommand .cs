using MediatR;

namespace Workspace_Management_System.Application.Features.Sessions.Services.Commands.DeleteService;

public class DeleteServiceCommand : IRequest<bool>
{
    public int SessionId { get; set; }
    public int ServiceId { get; set; }
}