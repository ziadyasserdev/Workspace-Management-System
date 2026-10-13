using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Sessions.Commands.ChangeSessionWorkspace
{
    public class ChangeSessionWorkspaceCommandValidator
       : AbstractValidator<ChangeSessionWorkspaceCommand>
    {
        public ChangeSessionWorkspaceCommandValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.SessionId)
                .GreaterThan(0)
                .WithMessage(
                    localizer["SessionIdMustBeGreaterThanZero"].Value);

            RuleFor(x => x.NewWorkspaceId)
                .GreaterThan(0)
                .WithMessage(
                    localizer["NewWorkspaceIdMustBeGreaterThanZero"].Value);
        }
    }
}
