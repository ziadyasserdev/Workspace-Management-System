using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.PaginatedResults;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Features.Sessions.Dtos;

namespace Workspace_Management_System.Application.Features.Sessions.Queries.GetActiveSessions
{
    public class GetActiveSessionsQuery
     : IRequest<Result<PaginatedResult<ActiveSessionDto>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string? Search { get; set; }
        public int? WorkspaceId { get; set; }
    }
}
