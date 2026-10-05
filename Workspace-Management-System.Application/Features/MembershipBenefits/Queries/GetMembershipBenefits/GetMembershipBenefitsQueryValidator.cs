using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Application.Features.MembershipBenefits.Queries.GetMembershipBenefits
{
    public class GetMembershipBenefitsQueryValidator
       : AbstractValidator<GetMembershipBenefitsQuery>
    {
        public GetMembershipBenefitsQueryValidator()
        {
            RuleFor(x => x.MembershipId)
                .GreaterThan(0)
                .WithMessage("Membership ID must be greater than 0.");
        }
    }
}
