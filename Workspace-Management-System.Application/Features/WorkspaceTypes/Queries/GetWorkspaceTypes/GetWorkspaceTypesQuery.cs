using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.PaginatedResults;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Features.WorkspaceTypes.Dtos;
using Workspace_Management_System.Application.Features.WorkspaceTypes.Queries.GetWorkspaceTypes;

namespace Workspace_Management_System.Application.Features.WorkspaceTypes.Queries.GetWorkspaceTypes
{
    public class WorkspaceTypeListDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public bool IsActive { get; set; }
    }
}
public class GetWorkspaceTypesQuery
         : IRequest<Result<PaginatedResult<WorkspaceTypeListDto>>>
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? Search { get; set; }

        public bool? IsActive { get; set; }
    }
