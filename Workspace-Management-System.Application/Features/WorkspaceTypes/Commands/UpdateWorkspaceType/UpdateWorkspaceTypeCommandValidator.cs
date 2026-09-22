using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Application.Features.WorkspaceTypes.Commands.UpdateWorkspaceType
{
    public class UpdateWorkspaceTypeCommandValidator
        : AbstractValidator<UpdateWorkspaceTypeCommand>
    {
        public UpdateWorkspaceTypeCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Workspace type ID must be greater than 0.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Workspace type name is required.")
                .MaximumLength(100)
                .WithMessage("Workspace type name cannot exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Workspace type description cannot exceed 500 characters.");
        }
    }
}
