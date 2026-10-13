using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.MembershipBenefits.Queries.GetMembershipBenefits
{
    public class GetMembershipBenefitsQueryValidator
        : AbstractValidator<GetMembershipBenefitsQuery>
    {
        public GetMembershipBenefitsQueryValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.MembershipId)
                .GreaterThan(0)
                .WithMessage(
                    localizer["MembershipIdMustBeGreaterThanZero"].Value);
        }
    }
}
