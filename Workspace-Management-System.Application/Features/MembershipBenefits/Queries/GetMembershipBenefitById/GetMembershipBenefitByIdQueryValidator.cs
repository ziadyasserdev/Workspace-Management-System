using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Application.Features.MembershipBenefits.Queries.GetMembershipBenefitById
{
    public class GetMembershipBenefitByIdQueryValidator
     : AbstractValidator<GetMembershipBenefitByIdQuery>
    {
        public GetMembershipBenefitByIdQueryValidator()
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
