using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.WorkspaceTypes.Commands.RestoreWorkspaceType
{
    public class RestoreWorkspaceTypeCommandValidator
         : AbstractValidator<RestoreWorkspaceTypeCommand>
    {
        public RestoreWorkspaceTypeCommandValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(
                    localizer["WorkspaceTypeIdMustBeGreaterThanZero"]);
        }
    }
}
