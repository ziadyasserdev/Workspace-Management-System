using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Employees.Queries.GetEmployees
{
    public class GetEmployeesQueryValidator
         : AbstractValidator<GetEmployeesQuery>
    {
        public GetEmployeesQueryValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage(
                    localizer["PageNumberMustBeGreaterThanZero"].Value);

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage(
                    localizer["PageSizeMustBeBetween1And100"].Value);

            RuleFor(x => x.Search)
                .MaximumLength(100)
                .When(x => !string.IsNullOrWhiteSpace(x.Search))
                .WithMessage(
                    localizer["EmployeeSearchMaxLength"].Value);

            RuleFor(x => x.WorkspaceId)
                .GreaterThan(0)
                .When(x => x.WorkspaceId.HasValue)
                .WithMessage(
                    localizer["WorkspaceIdMustBeGreaterThanZero"].Value);

            RuleFor(x => x.Status)
                .IsInEnum()
                .When(x => x.Status.HasValue)
                .WithMessage(
                    localizer["InvalidEmployeeStatus"].Value);
        }
    }
}
