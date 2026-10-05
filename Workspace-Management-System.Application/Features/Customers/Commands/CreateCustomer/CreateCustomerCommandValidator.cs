using FluentValidation;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommandValidator
        : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(x => x.FullNameEn)
                .NotEmpty()
                .WithMessage("Customer English full name is required.")
                .MaximumLength(150)
                .WithMessage("Customer English full name must not exceed 150 characters.");

            RuleFor(x => x.FullNameAr)
                .NotEmpty()
                .WithMessage("Customer Arabic full name is required.")
                .MaximumLength(150)
                .WithMessage("Customer Arabic full name must not exceed 150 characters.");

            RuleFor(x => x.MobileNumber)
                .NotEmpty()
                .WithMessage("Mobile number is required.")
                .Matches(@"^01[0125][0-9]{8}$")
                .WithMessage("Mobile number must be a valid Egyptian mobile number.");

            RuleFor(x => x.CustomerType)
                .IsInEnum()
                .WithMessage("Invalid customer type.");

            RuleFor(x => x.Email)
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")
                .When(x => !string.IsNullOrEmpty(x.Email))
                .WithMessage("Email must be valid.");

            RuleFor(x => x.CompanyId)
                .NotNull()
                .When(x => x.CustomerType == CustomerType.Corporate)
                .WithMessage("Corporate customers must be associated with a company.");

            RuleFor(x => x.CompanyId)
                .Null()
                .When(x =>
                    x.CustomerType == CustomerType.Individual ||
                    x.CustomerType == CustomerType.Member ||
                    x.CustomerType == CustomerType.WalkIn)
                .WithMessage("This customer type cannot be associated with a company.");

            RuleFor(x => x.NotesEn)
                .MaximumLength(500)
                .WithMessage("English notes must not exceed 500 characters.");

            RuleFor(x => x.NotesAr)
                .MaximumLength(500)
                .WithMessage("Arabic notes must not exceed 500 characters.");
        }
    }
}