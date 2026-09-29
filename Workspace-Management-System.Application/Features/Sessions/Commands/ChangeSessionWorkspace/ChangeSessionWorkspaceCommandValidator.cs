using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Application.Features.Sessions.Commands.ChangeSessionWorkspace
{
    public class ChangeSessionWorkspaceCommandValidator
       : AbstractValidator<ChangeSessionWorkspaceCommand>
    {
        public ChangeSessionWorkspaceCommandValidator()
        {
            RuleFor(x => x.SessionId)
                .GreaterThan(0)
                .WithMessage("Session id must be greater than 0.");

            RuleFor(x => x.NewWorkspaceId)
                .GreaterThan(0)
                .WithMessage("New workspace id must be greater than 0.");
        }
    }
}
