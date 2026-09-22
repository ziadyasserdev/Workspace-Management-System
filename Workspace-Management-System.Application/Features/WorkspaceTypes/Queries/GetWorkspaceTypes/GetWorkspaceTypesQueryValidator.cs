using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Application.Features.WorkspaceTypes.Queries.GetWorkspaceTypes
{
    public class GetWorkspaceTypesQueryValidator
       : AbstractValidator<GetWorkspaceTypesQuery>
    {
        public GetWorkspaceTypesQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than 0.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("Page size must be between 1 and 100.");

            RuleFor(x => x.Search)
                .MaximumLength(100)
                .WithMessage("Search cannot exceed 100 characters.");
        }
    }
}
