using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.Workspaces.Commands.CreateWorkspace
{
    public class CreateWorkspaceCommand : IRequest<Result<int>>
    {
        public int WorkspaceTypeId { get; set; }

        public string Name { get; set; } = null!;

        public string Code { get; set; } = null!;

        public string? Floor { get; set; }

        public string? Location { get; set; }

        public int Capacity { get; set; }

        public string? Description { get; set; }
    }
}
