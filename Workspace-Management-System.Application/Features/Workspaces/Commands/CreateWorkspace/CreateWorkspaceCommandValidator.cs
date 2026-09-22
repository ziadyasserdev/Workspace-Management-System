using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Application.Features.Workspaces.Commands.CreateWorkspace
{
    public class CreateWorkspaceCommandValidator
        : AbstractValidator<CreateWorkspaceCommand>
    {
        public CreateWorkspaceCommandValidator()
        {
            RuleFor(x => x.WorkspaceTypeId)
                .GreaterThan(0)
                .WithMessage("Workspace type ID must be greater than 0.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Workspace name is required.")
                .MaximumLength(150)
                .WithMessage("Workspace name cannot exceed 150 characters.");

            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Workspace code is required.")
                .MaximumLength(50)
                .WithMessage("Workspace code cannot exceed 50 characters.");

            RuleFor(x => x.Floor)
                .MaximumLength(50)
                .WithMessage("Floor cannot exceed 50 characters.");

            RuleFor(x => x.Location)
                .MaximumLength(200)
                .WithMessage("Location cannot exceed 200 characters.");

            RuleFor(x => x.Capacity)
                .GreaterThan(0)
                .WithMessage("Workspace capacity must be greater than 0.");

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Workspace description cannot exceed 500 characters.");
        }
    }
}
