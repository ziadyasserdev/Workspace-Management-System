using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.MembershipBenefits.Commands.CreateMembershipBenefit
{
    public class CreateMembershipBenefitCommandValidator
    : AbstractValidator<CreateMembershipBenefitCommand>
    {
        public CreateMembershipBenefitCommandValidator()
        {
            RuleFor(x => x.MembershipId)
                .GreaterThan(0)
                .WithMessage("Membership ID must be greater than 0.");

            RuleFor(x => x.BenefitType)
                .IsInEnum()
                .WithMessage("Invalid benefit type.");

            RuleFor(x => x.Hours)
                .GreaterThan(0)
                .When(x =>
                    x.BenefitType == BenefitType.WorkspaceHours ||
                    x.BenefitType == BenefitType.MeetingRoomHours)
                .WithMessage("Hours must be greater than 0.");
            RuleFor(x => x.Hours)
    .Null()
    .When(x =>
        x.BenefitType != BenefitType.WorkspaceHours &&
        x.BenefitType != BenefitType.MeetingRoomHours)
    .WithMessage(
        "Hours should only be provided for hour-based benefits.");

            RuleFor(x => x.DiscountPercentage)
                .Null()
                .When(x =>
                    x.BenefitType != BenefitType.ProductDiscount &&
                    x.BenefitType != BenefitType.ServiceDiscount)
                .WithMessage(
                    "Discount percentage should only be provided for discount benefits.");

            RuleFor(x => x.DiscountPercentage)
                .InclusiveBetween(0.01m, 100m)
                .When(x =>
                    x.BenefitType == BenefitType.ProductDiscount ||
                    x.BenefitType == BenefitType.ServiceDiscount)
                .WithMessage(
                    "Discount percentage must be greater than 0 and less than or equal to 100.");

            RuleFor(x => x.Description)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrWhiteSpace(x.Description))
                .WithMessage(
                    "Description must not exceed 1000 characters.");
        }
    }
}
