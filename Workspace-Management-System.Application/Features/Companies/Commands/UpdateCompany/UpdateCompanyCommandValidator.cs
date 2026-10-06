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
                .WithMessage("Company ID must be greater than 0.");

            RuleFor(x => x.NameEn)
                .NotEmpty()
                .WithMessage("Company English name is required.")
                .MaximumLength(150)
                .WithMessage("Company English name must not exceed 150 characters.");

            RuleFor(x => x.NameAr)
                .NotEmpty()
                .WithMessage("Company Arabic name is required.")
                .MaximumLength(150)
                .WithMessage("Company Arabic name must not exceed 150 characters.");

            RuleFor(x => x.ContactPerson)
                .NotEmpty()
                .WithMessage("Contact person is required.")
                .MaximumLength(150)
                .WithMessage("Contact person must not exceed 150 characters.");

            RuleFor(x => x.Phone)
                .NotEmpty()
                .WithMessage("Mobile number is required.")
                .Matches(@"^01[0125][0-9]{8}$")
                .WithMessage("Mobile number must be a valid Egyptian mobile number.");

            RuleFor(x => x.Email)
                .MaximumLength(150)
                .When(x => !string.IsNullOrWhiteSpace(x.Email))
                .WithMessage("Email must not exceed 150 characters.");

            RuleFor(x => x.Email)
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")
                .When(x => !string.IsNullOrWhiteSpace(x.Email))
                .WithMessage("Email must be valid.");

            RuleFor(x => x.TaxNumber)
                .MaximumLength(50)
                .When(x => !string.IsNullOrWhiteSpace(x.TaxNumber))
                .WithMessage("Tax number must not exceed 50 characters.");

            RuleFor(x => x.TaxInformationEn)
                .MaximumLength(500)
                .When(x => !string.IsNullOrWhiteSpace(x.TaxInformationEn))
                .WithMessage("English tax information must not exceed 500 characters.");

            RuleFor(x => x.TaxInformationAr)
                .MaximumLength(500)
                .When(x => !string.IsNullOrWhiteSpace(x.TaxInformationAr))
                .WithMessage("Arabic tax information must not exceed 500 characters.");

            RuleFor(x => x.ContractDetailsEn)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrWhiteSpace(x.ContractDetailsEn))
                .WithMessage("English contract details must not exceed 1000 characters.");

            RuleFor(x => x.ContractDetailsAr)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrWhiteSpace(x.ContractDetailsAr))
                .WithMessage("Arabic contract details must not exceed 1000 characters.");

            RuleFor(x => x.CreditLimit)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Credit limit cannot be negative.");
        }
    }
}