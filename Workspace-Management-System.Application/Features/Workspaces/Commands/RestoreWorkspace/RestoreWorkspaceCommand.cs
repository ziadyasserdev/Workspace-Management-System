using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.Workspaces.Commands.RestoreWorkspace
{
    public class RestoreWorkspaceCommand : IRequest<Result<bool>>
    {
        public int Id { get; set; }
    }
}
