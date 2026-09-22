using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.PaginatedResults;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Features.Workspaces.Dtos;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Workspaces.Queries.GetWorkspaces
{
    public class GetWorkspacesQuery
      : IRequest<Result<PaginatedResult<WorkspaceDto>>>
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? Search { get; set; }

        public int? WorkspaceTypeId { get; set; }

        public WorkspaceStatus? Status { get; set; }

        public bool? IsActive { get; set; }
    }
}
