using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.WorkspaceTypes.Queries.GetWorkspaceTypes
{
    public class GetWorkspaceTypesQueryValidator
      : AbstractValidator<GetWorkspaceTypesQuery>
    {
        public GetWorkspaceTypesQueryValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage(localizer["PageNumberMustBeGreaterThanZero"]);

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage(localizer["PageSizeMustBeBetween1And100"]);

            RuleFor(x => x.Search)
                .MaximumLength(100)
                .WithMessage(localizer["WorkspaceTypeSearchMaxLength"]);
        }
    }
}
