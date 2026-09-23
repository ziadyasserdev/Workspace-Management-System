using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.Employees.Commands.AssignEmployeeToWorkspace
{
    public class AssignEmployeeToWorkspaceCommand : IRequest<Result<bool>>
    {
        public int EmployeeId { get; set; }

        public int WorkspaceId { get; set; }
    }
}
