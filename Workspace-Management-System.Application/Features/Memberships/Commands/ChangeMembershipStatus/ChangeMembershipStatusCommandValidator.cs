using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Application.Features.Memberships.Commands.ChangeMembershipStatus
{
    public class ChangeMembershipStatusCommandValidator
      : AbstractValidator<ChangeMembershipStatusCommand>
    {
        public ChangeMembershipStatusCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Membership ID must be greater than 0.");
        }
    }
}
