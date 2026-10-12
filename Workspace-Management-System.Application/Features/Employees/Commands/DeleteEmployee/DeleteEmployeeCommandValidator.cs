using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Employees.Commands.DeleteEmployee
{
    public class DeleteEmployeeCommandValidator
          : AbstractValidator<DeleteEmployeeCommand>
    {
        public DeleteEmployeeCommandValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0)
                .WithMessage(
                    localizer["EmployeeIdMustBeGreaterThanZero"].Value);
        }
    }
}
