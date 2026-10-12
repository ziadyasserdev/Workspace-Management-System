using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Employees.Commands.UpdateEmployee
{
    public class UpdateEmployeeCommandValidator
           : AbstractValidator<UpdateEmployeeCommand>
    {
        public UpdateEmployeeCommandValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0)
                .WithMessage(
                    localizer["EmployeeIdMustBeGreaterThanZero"].Value);

            RuleFor(x => x.FirstNameEn)
                .NotEmpty()
                .WithMessage(localizer["EmployeeFirstNameEnRequired"].Value)
                .MaximumLength(100)
                .WithMessage(localizer["EmployeeFirstNameEnMaxLength"].Value);

            RuleFor(x => x.FirstNameAr)
                .NotEmpty()
                .WithMessage(localizer["EmployeeFirstNameArRequired"].Value)
                .MaximumLength(100)
                .WithMessage(localizer["EmployeeFirstNameArMaxLength"].Value);

            RuleFor(x => x.LastNameEn)
                .NotEmpty()
                .WithMessage(localizer["EmployeeLastNameEnRequired"].Value)
                .MaximumLength(100)
                .WithMessage(localizer["EmployeeLastNameEnMaxLength"].Value);

            RuleFor(x => x.LastNameAr)
                .NotEmpty()
                .WithMessage(localizer["EmployeeLastNameArRequired"].Value)
                .MaximumLength(100)
                .WithMessage(localizer["EmployeeLastNameArMaxLength"].Value);

            RuleFor(x => x.Phone)
                .NotEmpty()
                .WithMessage(localizer["PhoneNumberRequired"].Value)
                .MaximumLength(20)
                .WithMessage(localizer["EmployeePhoneMaxLength"].Value)
                .Matches(@"^\+?\d{10,15}$")
                .WithMessage(localizer["InvalidPhoneNumber"].Value);

            RuleFor(x => x.Email)
                .EmailAddress()
                .When(x => !string.IsNullOrWhiteSpace(x.Email))
                .WithMessage(localizer["InvalidEmailFormat"].Value);

            RuleFor(x => x.Email)
                .MaximumLength(256)
                .When(x => !string.IsNullOrWhiteSpace(x.Email))
                .WithMessage(localizer["EmailMaxLength"].Value);
        }
    }
}
