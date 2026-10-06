
using FluentValidation;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Resources;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommandValidator
        : AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerCommandValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(localizer["CustomerIdGreaterThanZero"]);

            RuleFor(x => x.FullNameEn)
                .NotEmpty()
                .WithMessage(localizer["CustomerEnglishFullNameRequired"])
                .MaximumLength(150)
                .WithMessage(localizer["CustomerEnglishFullNameMaxLength"]);

            RuleFor(x => x.FullNameAr)
                .NotEmpty()
                .WithMessage(localizer["CustomerArabicFullNameRequired"])
                .MaximumLength(150)
                .WithMessage(localizer["CustomerArabicFullNameMaxLength"]);

            RuleFor(x => x.MobileNumber)
                .NotEmpty()
                .WithMessage(localizer["MobileNumberRequired"])
                .Matches(@"^01[0125][0-9]{8}$")
                .WithMessage(localizer["InvalidEgyptianMobileNumber"]);

            RuleFor(x => x.Email)
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")
                .When(x => !string.IsNullOrEmpty(x.Email))
                .WithMessage(localizer["InvalidEmail"]);

            RuleFor(x => x.CustomerType)
                .IsInEnum()
                .WithMessage(localizer["InvalidCustomerType"]);

            RuleFor(x => x.CompanyId)
                .NotNull()
                .When(x => x.CustomerType == CustomerType.Corporate)
                .WithMessage(localizer["CorporateCustomerRequiresCompany"]);

            RuleFor(x => x.CompanyId)
                .Null()
                .When(x =>
                    x.CustomerType == CustomerType.Individual ||
                    x.CustomerType == CustomerType.Member ||
                    x.CustomerType == CustomerType.WalkIn)
                .WithMessage(localizer["CustomerTypeCannotBeAssociatedWithCompany"]);

            RuleFor(x => x.NotesEn)
                .MaximumLength(500)
                .WithMessage(localizer["EnglishNotesMaxLength"]);

            RuleFor(x => x.NotesAr)
                .MaximumLength(500)
                .WithMessage(localizer["ArabicNotesMaxLength"]);
        }
    }
}
