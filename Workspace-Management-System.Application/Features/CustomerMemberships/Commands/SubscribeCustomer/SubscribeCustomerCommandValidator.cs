using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.CustomerMemberships.Commands.SubscribeCustomer
{
    public class SubscribeCustomerCommandValidator
        : AbstractValidator<SubscribeCustomerCommand>
    {
        public SubscribeCustomerCommandValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.CustomerId)
                .GreaterThan(0)
                .WithMessage(
                    localizer["CustomerIdMustBeGreaterThanZero"].Value);

            RuleFor(x => x.MembershipId)
                .GreaterThan(0)
                .WithMessage(
                    localizer["MembershipIdMustBeGreaterThanZero"].Value);
        }
    }
}
