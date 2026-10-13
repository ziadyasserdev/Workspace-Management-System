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
      : IRequest<Result<PaginatedResult<WorkspaceListDto>>>
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? Search { get; set; }

        public int? WorkspaceTypeId { get; set; }

        public WorkspaceStatus? Status { get; set; }

        public bool? IsActive { get; set; }
    }
    public class WorkspaceListDto
    {
        public int Id { get; set; }

        public int WorkspaceTypeId { get; set; }

        public string WorkspaceTypeName { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Code { get; set; } = null!;

        public string? Floor { get; set; }

        public string? Location { get; set; }

        public int Capacity { get; set; }

        public WorkspaceStatus Status { get; set; }

        public string? Description { get; set; }

        public bool IsActive { get; set; }
    }
}
