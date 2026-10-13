using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Memberships.Queries.GetMembershipById
{
    public class GetMembershipByIdQueryValidator
       : AbstractValidator<GetMembershipByIdQuery>
    {
        public GetMembershipByIdQueryValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(
                    localizer["MembershipIdMustBeGreaterThanZero"].Value);
        }
    }
}
