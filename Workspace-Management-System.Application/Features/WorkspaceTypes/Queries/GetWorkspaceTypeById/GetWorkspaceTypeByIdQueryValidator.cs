using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Application.Features.WorkspaceTypes.Queries.GetWorkspaceTypeById
{
    public class GetWorkspaceTypeByIdQueryValidator
        : AbstractValidator<GetWorkspaceTypeByIdQuery>
    {
        public GetWorkspaceTypeByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Workspace type ID must be greater than 0.");
        }
    }
}
