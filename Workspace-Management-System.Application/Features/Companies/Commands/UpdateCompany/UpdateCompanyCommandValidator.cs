using FluentValidation;

namespace Workspace_Management_System.Application.Features.Companies.Commands.UpdateCompany
{
    public class UpdateCompanyCommandValidator
        : AbstractValidator<UpdateCompanyCommand>
    {
        public UpdateCompanyCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Company ID must be greater than 0");

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
                .WithMessage("Phone number is required")
                .MaximumLength(20)
                .WithMessage("Phone number must not exceed 20 characters");

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
                .When(x => !string.IsNullOrWhiteSpace(x.TaxNumber))
                .WithMessage("Tax number must not exceed 50 characters");

            RuleFor(x => x.TaxInformation)
                .MaximumLength(500)
                .When(x => !string.IsNullOrWhiteSpace(x.TaxInformation))
                .WithMessage("Tax information must not exceed 500 characters");

            RuleFor(x => x.ContractDetails)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrWhiteSpace(x.ContractDetails))
                .WithMessage("Contract details must not exceed 1000 characters");

            RuleFor(x => x.CreditLimit)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Credit limit cannot be negative");
        }
    }
}