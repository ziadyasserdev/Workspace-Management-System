using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Workspaces.Commands.ChangeWorkspaceStatus
{
    public class ChangeWorkspaceStatusCommandValidator
       : AbstractValidator<ChangeWorkspaceStatusCommand>
    {
        public ChangeWorkspaceStatusCommandValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(
                    localizer["WorkspaceIdMustBeGreaterThanZero"].Value);

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage(
                    localizer["InvalidWorkspaceStatus"].Value);
        }
    }
}
