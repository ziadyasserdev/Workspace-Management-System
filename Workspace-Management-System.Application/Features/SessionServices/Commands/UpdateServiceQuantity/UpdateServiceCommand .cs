using MediatR;
using Workspace_Management_System.Application.Features.Sessions.Services.Dtos;

namespace Workspace_Management_System.Application.Features.Sessions.Services.Commands.UpdateService;

public class UpdateServiceCommand : IRequest<SessionServiceResponseDto>
{
    public int SessionId { get; set; }
    public int ServiceId { get; set; }
    public decimal Quantity { get; set; }
}