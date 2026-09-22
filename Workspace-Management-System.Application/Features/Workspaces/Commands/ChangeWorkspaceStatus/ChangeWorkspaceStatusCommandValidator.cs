using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Application.Features.Workspaces.Commands.ChangeWorkspaceStatus
{
    public class ChangeWorkspaceStatusCommandValidator
       : AbstractValidator<ChangeWorkspaceStatusCommand>
    {
        public ChangeWorkspaceStatusCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Workspace ID must be greater than 0.");

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Invalid workspace status.");
        }
    }
}
