using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Application.Features.CustomerMemberships.Commands.SubscribeCustomer
{
    public class SubscribeCustomerCommandValidator
      : AbstractValidator<SubscribeCustomerCommand>
    {
        public SubscribeCustomerCommandValidator()
        {
            RuleFor(x => x.CustomerId)
                .GreaterThan(0)
                .WithMessage("Customer ID must be greater than 0.");

            RuleFor(x => x.MembershipId)
                .GreaterThan(0)
                .WithMessage("Membership ID must be greater than 0.");
        }
    }
}
