using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Application.Features.WorkspaceTypes.Commands.ChangeWorkspaceTypeStatus
{
    public class ChangeWorkspaceTypeStatusCommandValidator
      : AbstractValidator<ChangeWorkspaceTypeStatusCommand>
    {
        public ChangeWorkspaceTypeStatusCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Workspace type ID must be greater than 0.");
        }
    }
}
