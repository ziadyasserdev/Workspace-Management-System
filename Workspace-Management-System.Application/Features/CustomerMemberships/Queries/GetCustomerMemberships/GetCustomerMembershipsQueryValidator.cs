using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.CustomerMemberships.Queries.GetCustomerMemberships
{

    public class GetCustomerMembershipsQueryValidator
         : AbstractValidator<GetCustomerMembershipsQuery>
    {
        public GetCustomerMembershipsQueryValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.CustomerId)
                .GreaterThan(0)
                .WithMessage(
                    localizer["CustomerIdMustBeGreaterThanZero"].Value);
        }
    }
}
