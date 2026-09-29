using MediatR;
using Workspace_Management_System.Application.Common.PaginatedResults;
using Workspace_Management_System.Application.Features.Sessions.Services.Dtos;

namespace Workspace_Management_System.Application.Features.Sessions.Services.Queries.GetSessionServices;

public class GetSessionServicesQuery : IRequest<PaginatedResult<SessionServiceResponseDto>>
{
    public int SessionId { get; set; }

    public string? Search { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}