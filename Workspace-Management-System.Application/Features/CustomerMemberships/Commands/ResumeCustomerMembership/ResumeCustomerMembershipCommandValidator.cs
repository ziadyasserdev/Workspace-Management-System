using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Application.Features.CustomerMemberships.Commands.ResumeCustomerMembership
{
    public class ResumeCustomerMembershipCommandValidator
    : AbstractValidator<ResumeCustomerMembershipCommand>
    {
        public ResumeCustomerMembershipCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Customer membership ID must be greater than 0.");
        }
    }
}
