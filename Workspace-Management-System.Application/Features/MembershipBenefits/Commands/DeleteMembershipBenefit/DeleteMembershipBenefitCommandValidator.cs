using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Application.Features.MembershipBenefits.Commands.DeleteMembershipBenefit
{
    public class DeleteMembershipBenefitCommandValidator
        : AbstractValidator<DeleteMembershipBenefitCommand>
    {
        public DeleteMembershipBenefitCommandValidator()
        {
            RuleFor(x => x.MembershipId)
                .GreaterThan(0)
                .WithMessage("Membership ID must be greater than 0.");

            RuleFor(x => x.BenefitId)
                .GreaterThan(0)
                .WithMessage("Benefit ID must be greater than 0.");
        }
    }
}
