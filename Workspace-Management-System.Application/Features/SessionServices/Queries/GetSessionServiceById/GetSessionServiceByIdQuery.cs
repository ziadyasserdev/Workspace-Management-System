using MediatR;
using Workspace_Management_System.Application.Features.Sessions.Services.Dtos;

namespace Workspace_Management_System.Application.Features.Sessions.Services.Queries.GetSessionServiceById;

public class GetSessionServiceByIdQuery : IRequest<SessionServiceResponseDto>
{
    public int Id { get; set; }
}