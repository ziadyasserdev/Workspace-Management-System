using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Memberships.Commands.UpdateMembership
{
    public class UpdateMembershipCommandValidator
      : AbstractValidator<UpdateMembershipCommand>
    {
        public UpdateMembershipCommandValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(
                    localizer["MembershipIdMustBeGreaterThanZero"].Value);

            RuleFor(x => x.NameEn)
                .NotEmpty()
                .WithMessage(
                    localizer["MembershipNameEnRequired"].Value)
                .MaximumLength(100)
                .WithMessage(
                    localizer["MembershipNameEnMaxLength"].Value);

            RuleFor(x => x.NameAr)
                .NotEmpty()
                .WithMessage(
                    localizer["MembershipNameArRequired"].Value)
                .MaximumLength(100)
                .WithMessage(
                    localizer["MembershipNameArMaxLength"].Value);

            RuleFor(x => x.DescriptionEn)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrWhiteSpace(x.DescriptionEn))
                .WithMessage(
                    localizer["MembershipDescriptionEnMaxLength"].Value);

            RuleFor(x => x.DescriptionAr)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrWhiteSpace(x.DescriptionAr))
                .WithMessage(
                    localizer["MembershipDescriptionArMaxLength"].Value);

            RuleFor(x => x.DurationDays)
                .GreaterThan(0)
                .WithMessage(
                    localizer["MembershipDurationMustBeGreaterThanZero"].Value);

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0)
                .WithMessage(
                    localizer["MembershipPriceCannotBeNegative"].Value);
        }
    }
}
