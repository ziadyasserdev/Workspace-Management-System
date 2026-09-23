using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Application.Features.Employees.Commands.AssignEmployeeToWorkspace
{

    public class AssignEmployeeToWorkspaceCommandValidator
        : AbstractValidator<AssignEmployeeToWorkspaceCommand>
    {
        public AssignEmployeeToWorkspaceCommandValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0)
                .WithMessage("EmployeeId must be greater than 0.");

            RuleFor(x => x.WorkspaceId)
                .GreaterThan(0)
                .WithMessage("WorkspaceId must be greater than 0.");
        }
    }
}
