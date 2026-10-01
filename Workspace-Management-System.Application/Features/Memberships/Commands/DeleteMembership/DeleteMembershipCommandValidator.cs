using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Application.Features.Memberships.Commands.DeleteMembership
{
    public class DeleteMembershipCommandValidator
     : AbstractValidator<DeleteMembershipCommand>
    {
        public DeleteMembershipCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Membership ID must be greater than 0.");
        }
    }
}
