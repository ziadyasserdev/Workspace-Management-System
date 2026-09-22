using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Features.WorkspaceTypes.Dtos;

namespace Workspace_Management_System.Application.Features.WorkspaceTypes.Queries.GetWorkspaceTypeById
{
    public class GetWorkspaceTypeByIdQuery : IRequest<Result<WorkspaceTypeDto>>
    {
        public int Id { get; set; }

        public GetWorkspaceTypeByIdQuery(int id)
        {
            Id = id;
        }
    }
}
