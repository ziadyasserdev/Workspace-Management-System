using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Workspaces.Queries.GetWorkspaceById
{

    public class GetWorkspaceByIdQueryValidator
        : AbstractValidator<GetWorkspaceByIdQuery>
    {
        public GetWorkspaceByIdQueryValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(
                    localizer["WorkspaceIdMustBeGreaterThanZero"].Value);
        }
    }
}
