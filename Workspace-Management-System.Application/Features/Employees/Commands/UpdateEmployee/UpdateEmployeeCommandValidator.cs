using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Application.Features.Employees.Commands.UpdateEmployee
{
    public class UpdateEmployeeCommandValidator
      : AbstractValidator<UpdateEmployeeCommand>
    {
        public UpdateEmployeeCommandValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0)
                .WithMessage("Employee id must be greater than 0.");

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage("First name is required and must not exceed 100 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage("Last name is required and must not exceed 100 characters.");

            RuleFor(x => x.Phone)
                .NotEmpty()
                .MaximumLength(20)
                .WithMessage("Phone is required and must not exceed 20 characters.");

            RuleFor(x => x.Email)
                .EmailAddress()
                .When(x => !string.IsNullOrWhiteSpace(x.Email))
                .WithMessage("Invalid email address.");

            RuleFor(x => x.Email)
                .MaximumLength(256)
                .When(x => !string.IsNullOrWhiteSpace(x.Email))
                .WithMessage("Email must not exceed 256 characters.");
        }
    }
}
