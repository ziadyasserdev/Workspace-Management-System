using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Authentications.Commands.RegisterUser
{
    public class RegisterUserCommandValidator
          : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.FirstNameEn)
                .NotEmpty()
                .WithMessage(localizer["EmployeeFirstNameEnRequired"].Value)
                .MinimumLength(3)
                .WithMessage(localizer["EmployeeFirstNameEnMinLength"].Value);

            RuleFor(x => x.FirstNameAr)
                .NotEmpty()
                .WithMessage(localizer["EmployeeFirstNameArRequired"].Value)
                .MinimumLength(3)
                .WithMessage(localizer["EmployeeFirstNameArMinLength"].Value);

            RuleFor(x => x.LastNameEn)
                .NotEmpty()
                .WithMessage(localizer["EmployeeLastNameEnRequired"].Value)
                .MinimumLength(3)
                .WithMessage(localizer["EmployeeLastNameEnMinLength"].Value);

            RuleFor(x => x.LastNameAr)
                .NotEmpty()
                .WithMessage(localizer["EmployeeLastNameArRequired"].Value)
                .MinimumLength(3)
                .WithMessage(localizer["EmployeeLastNameArMinLength"].Value);

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage(localizer["EmailRequired"].Value)
                .EmailAddress()
                .WithMessage(localizer["InvalidEmailFormat"].Value)
                .MaximumLength(256)
                .WithMessage(localizer["EmailMaxLength"].Value);

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .WithMessage(localizer["PhoneNumberRequired"].Value)
                .Matches(@"^\+?\d{10,15}$")
                .WithMessage(localizer["InvalidPhoneNumber"].Value);

            RuleFor(x => x.Gender)
                .IsInEnum()
                .WithMessage(localizer["InvalidGender"].Value);

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(localizer["PasswordRequired"].Value)
                .MinimumLength(8)
                .WithMessage(localizer["PasswordMinLength"].Value)
                .Matches("[A-Z]")
                .WithMessage(localizer["PasswordMustContainUppercase"].Value)
                .Matches("[a-z]")
                .WithMessage(localizer["PasswordMustContainLowercase"].Value)
                .Matches("[0-9]")
                .WithMessage(localizer["PasswordMustContainNumber"].Value);

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .WithMessage(localizer["ConfirmPasswordRequired"].Value)
                .Equal(x => x.Password)
                .WithMessage(localizer["PasswordsDoNotMatch"].Value);
        }
    }
}
