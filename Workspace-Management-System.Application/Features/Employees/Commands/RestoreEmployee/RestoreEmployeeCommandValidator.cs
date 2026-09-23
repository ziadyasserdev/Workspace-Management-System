using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Application.Features.Employees.Commands.RestoreEmployee
{
    public class RestoreEmployeeCommandValidator
      : AbstractValidator<RestoreEmployeeCommand>
    {
        public RestoreEmployeeCommandValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0)
                .WithMessage("Employee id must be greater than 0.");
        }
    }
}
