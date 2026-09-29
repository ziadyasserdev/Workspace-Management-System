using MediatR;
using Workspace_Management_System.Application.Common.PaginatedResults;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Features.Services.Dtos;

namespace Workspace_Management_System.Application.Features.Services.Queries.GetServices
{
    public class GetServicesQuery : IRequest<Result<PaginatedResult<ServiceResponseDto>>>
    {
        public string? Search { get; set; }

        public bool? IsActive { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
