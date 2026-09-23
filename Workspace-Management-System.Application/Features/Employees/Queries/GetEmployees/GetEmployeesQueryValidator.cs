using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Application.Features.Employees.Queries.GetEmployees
{
    public class GetEmployeesQueryValidator
    : AbstractValidator<GetEmployeesQuery>
    {
        public GetEmployeesQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than 0.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("Page size must be between 1 and 100.");

            RuleFor(x => x.Search)
                .MaximumLength(100)
                .When(x => !string.IsNullOrWhiteSpace(x.Search))
                .WithMessage("Search cannot exceed 100 characters.");

            RuleFor(x => x.WorkspaceId)
                .GreaterThan(0)
                .When(x => x.WorkspaceId.HasValue)
                .WithMessage("Workspace id must be greater than 0.");

            RuleFor(x => x.Status)
                .IsInEnum()
                .When(x => x.Status.HasValue)
                .WithMessage("Invalid employee status.");
        }
    }
}
