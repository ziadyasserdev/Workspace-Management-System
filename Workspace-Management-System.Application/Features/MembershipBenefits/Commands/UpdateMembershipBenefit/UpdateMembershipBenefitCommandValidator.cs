using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Resources;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.MembershipBenefits.Commands.UpdateMembershipBenefit
{
    public class UpdateMembershipBenefitCommandValidator
        : AbstractValidator<UpdateMembershipBenefitCommand>
    {
        public UpdateMembershipBenefitCommandValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.MembershipId)
                .GreaterThan(0)
                .WithMessage(localizer["MembershipIdMustBeGreaterThanZero"].Value);

            RuleFor(x => x.BenefitId)
                .GreaterThan(0)
                .WithMessage(localizer["MembershipBenefitIdMustBeGreaterThanZero"].Value);

            RuleFor(x => x.BenefitType)
                .IsInEnum()
                .WithMessage(localizer["InvalidBenefitType"].Value);

            RuleFor(x => x.Hours)
                .GreaterThan(0)
                .When(x =>
                    x.BenefitType == BenefitType.WorkspaceHours ||
                    x.BenefitType == BenefitType.MeetingRoomHours)
                .WithMessage(localizer["BenefitHoursMustBeGreaterThanZero"].Value);

            RuleFor(x => x.DiscountPercentage)
                .InclusiveBetween(0.01m, 100m)
                .When(x =>
                    x.BenefitType == BenefitType.ProductDiscount ||
                    x.BenefitType == BenefitType.ServiceDiscount)
                .WithMessage(localizer["DiscountPercentageMustBeBetween0And100"].Value);

            RuleFor(x => x.WorkspaceTypeId)
                .NotNull()
                .When(x =>
                    x.BenefitType == BenefitType.WorkspaceHours ||
                    x.BenefitType == BenefitType.MeetingRoomHours)
                .WithMessage(localizer["WorkspaceTypeRequiredForWorkspaceBenefits"].Value);

            RuleFor(x => x.Hours)
                .Null()
                .When(x =>
                    x.BenefitType != BenefitType.WorkspaceHours &&
                    x.BenefitType != BenefitType.MeetingRoomHours)
                .WithMessage(localizer["HoursOnlyForHourBasedBenefits"].Value);

            RuleFor(x => x.DiscountPercentage)
                .Null()
                .When(x =>
                    x.BenefitType != BenefitType.ProductDiscount &&
                    x.BenefitType != BenefitType.ServiceDiscount)
                .WithMessage(localizer["DiscountOnlyForDiscountBenefits"].Value);

            RuleFor(x => x.DescriptionEn)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrWhiteSpace(x.DescriptionEn))
                .WithMessage(localizer["MembershipBenefitDescriptionEnMaxLength"].Value);

            RuleFor(x => x.DescriptionAr)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrWhiteSpace(x.DescriptionAr))
                .WithMessage(localizer["MembershipBenefitDescriptionArMaxLength"].Value);
        }
    }
}