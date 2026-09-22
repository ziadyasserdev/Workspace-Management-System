using FluentValidation;

namespace Workspace_Management_System.Application.Features.Companies.Commands.CreateCompany
{
    public class CreateCompanyCommandValidator
        : AbstractValidator<CreateCompanyCommand>
    {
        public CreateCompanyCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Company name is required")
                .MaximumLength(150)
                .WithMessage("Company name must not exceed 150 characters");

            RuleFor(x => x.ContactPerson)
                .NotEmpty()
                .WithMessage("Contact person is required")
                .MaximumLength(150)
                .WithMessage("Contact person must not exceed 150 characters");

            RuleFor(x => x.Phone)
          .NotEmpty()
          .WithMessage("Mobile number is required")
          .Matches(@"^01[0125][0-9]{8}$")
          .WithMessage("Mobile number must be a valid Egyptian mobile number");

            RuleFor(x => x.Email)
             .MaximumLength(150)
             .When(x => !string.IsNullOrWhiteSpace(x.Email))
             .WithMessage("Email must not exceed 150 characters");

            RuleFor(x => x.Email)
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")
                .When(x => !string.IsNullOrWhiteSpace(x.Email))
                .WithMessage("Email must be valid");
            RuleFor(x => x.TaxNumber)
                .MaximumLength(50)
                .WithMessage("Tax number must not exceed 50 characters");

            RuleFor(x => x.TaxInformation)
                .MaximumLength(500)
                .WithMessage("Tax information must not exceed 500 characters");

            RuleFor(x => x.ContractDetails)
                .MaximumLength(1000)
                .WithMessage("Contract details must not exceed 1000 characters");

            RuleFor(x => x.CreditLimit)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Credit limit must not be negative");
        }
    }
}