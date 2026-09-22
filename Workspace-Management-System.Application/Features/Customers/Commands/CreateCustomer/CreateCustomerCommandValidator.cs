using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Application.Features.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage("Full name is required")
                .MaximumLength(150)
                .WithMessage("Full name must not exceed 150 characters");

            RuleFor(x => x.MobileNumber)
                .NotEmpty()
                .WithMessage("Mobile number is required")
                .MaximumLength(20)
                .WithMessage("Mobile number must not exceed 20 characters");

            RuleFor(x => x.MobileNumber)
            .NotEmpty()
            .WithMessage("Mobile number is required")
            .Matches(@"^01[0125][0-9]{8}$")
            .WithMessage("Mobile number must be a valid Egyptian mobile number");

            RuleFor(x => x.CustomerType)
                .NotEmpty()
                .WithMessage("Customer type is required")
                .MaximumLength(50)
                .WithMessage("Customer type must not exceed 50 characters");

            RuleFor(x => x.Notes)
                .MaximumLength(500)
                .WithMessage("Notes must not exceed 500 characters");
        }
    }
}