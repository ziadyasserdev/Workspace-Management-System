using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.WorkspaceTypes.Commands.ChangeWorkspaceTypeStatus
{
    public class ChangeWorkspaceTypeStatusCommand : IRequest<Result<bool>>
    {
        public int Id { get; set; }

        public bool IsActive { get; set; }
    }
}
