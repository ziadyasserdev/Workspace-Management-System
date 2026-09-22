using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Features.Workspaces.Dtos;

namespace Workspace_Management_System.Application.Features.Workspaces.Queries.GetWorkspaceById
{
    public class GetWorkspaceByIdQuery
      : IRequest<Result<WorkspaceDto>>
    {
        public int Id { get; set; }

        public GetWorkspaceByIdQuery(int id)
        {
            Id = id;
        }
    }
}
