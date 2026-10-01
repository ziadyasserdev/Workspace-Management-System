using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Application.Features.CustomerMemberships.Commands.RenewCustomerMembership
{
    public class RenewCustomerMembershipCommandValidator
    : AbstractValidator<RenewCustomerMembershipCommand>
    {
        public RenewCustomerMembershipCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Customer membership ID must be greater than 0.");
        }
    }
}
