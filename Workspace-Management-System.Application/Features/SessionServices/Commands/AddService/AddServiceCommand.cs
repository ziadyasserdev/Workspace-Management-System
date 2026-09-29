using MediatR;
using Workspace_Management_System.Application.Features.Sessions.Services.Dtos;

public class AddServiceCommand : IRequest<SessionServiceResponseDto>
{
    public int SessionId { get; set; }
    public int ServiceId { get; set; }
    public decimal Quantity { get; set; }
}