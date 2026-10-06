using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Companies.Commands.CreateCompany
{
    public class CreateCompanyCommandValidator
        : AbstractValidator<CreateCompanyCommand>
    {
        public CreateCompanyCommandValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.NameEn)
                .NotEmpty()
                .WithMessage(localizer["CompanyEnglishNameRequired"])
                .MaximumLength(150)
                .WithMessage(localizer["CompanyEnglishNameMaxLength"]);

            RuleFor(x => x.NameAr)
                .NotEmpty()
                .WithMessage(localizer["CompanyArabicNameRequired"])
                .MaximumLength(150)
                .WithMessage(localizer["CompanyArabicNameMaxLength"]);

            RuleFor(x => x.ContactPerson)
                .NotEmpty()
                .WithMessage(localizer["ContactPersonRequired"])
                .MaximumLength(150)
                .WithMessage(localizer["ContactPersonMaxLength"]);

            RuleFor(x => x.Phone)
                .NotEmpty()
                .WithMessage(localizer["MobileNumberRequired"])
                .Matches(@"^01[0125][0-9]{8}$")
                .WithMessage(localizer["InvalidEgyptianMobileNumber"]);

            RuleFor(x => x.Email)
                .MaximumLength(150)
                .When(x => !string.IsNullOrWhiteSpace(x.Email))
                .WithMessage(localizer["CompanyEmailMaxLength"]);

            RuleFor(x => x.Email)
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")
                .When(x => !string.IsNullOrWhiteSpace(x.Email))
                .WithMessage(localizer["InvalidEmail"]);

            RuleFor(x => x.TaxNumber)
                .MaximumLength(50)
                .WithMessage(localizer["TaxNumberMaxLength"]);

            RuleFor(x => x.TaxInformationEn)
                .MaximumLength(500)
                .WithMessage(localizer["EnglishTaxInformationMaxLength"]);

            RuleFor(x => x.TaxInformationAr)
                .MaximumLength(500)
                .WithMessage(localizer["ArabicTaxInformationMaxLength"]);

            RuleFor(x => x.ContractDetailsEn)
                .MaximumLength(1000)
                .WithMessage(localizer["EnglishContractDetailsMaxLength"]);

            RuleFor(x => x.ContractDetailsAr)
                .MaximumLength(1000)
                .WithMessage(localizer["ArabicContractDetailsMaxLength"]);

            RuleFor(x => x.CreditLimit)
                .GreaterThanOrEqualTo(0)
                .WithMessage(localizer["CreditLimitCannotBeNegative"]);
        }
    }
}

