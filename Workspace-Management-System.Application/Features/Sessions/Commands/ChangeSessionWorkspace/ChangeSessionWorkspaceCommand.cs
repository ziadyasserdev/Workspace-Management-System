using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.Sessions.Commands.ChangeSessionWorkspace
{
    public class ChangeSessionWorkspaceCommand : IRequest<Result<bool>>
    {
        public int SessionId { get; set; }
        public int NewWorkspaceId { get; set; }
    }
}
