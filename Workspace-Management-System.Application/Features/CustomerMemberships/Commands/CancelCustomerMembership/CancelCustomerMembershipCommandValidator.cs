using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Application.Features.CustomerMemberships.Commands.CancelCustomerMembership
{
    public class CancelCustomerMembershipCommandValidator
     : AbstractValidator<CancelCustomerMembershipCommand>
    {
        public CancelCustomerMembershipCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Customer membership ID must be greater than 0.");
        }
    }
}
