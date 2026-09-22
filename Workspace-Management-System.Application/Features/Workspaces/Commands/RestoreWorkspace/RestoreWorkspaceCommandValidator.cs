using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Application.Features.Workspaces.Commands.RestoreWorkspace
{
    public class RestoreWorkspaceCommandValidator
       : AbstractValidator<RestoreWorkspaceCommand>
    {
        public RestoreWorkspaceCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Workspace ID must be greater than 0.");
        }
    }
}
