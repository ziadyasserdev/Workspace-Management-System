using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Application.Features.Memberships.Commands.UpdateMembership
{
    public class UpdateMembershipCommandValidator
     : AbstractValidator<UpdateMembershipCommand>
    {
        public UpdateMembershipCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Membership ID must be greater than 0.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Membership name is required.")
                .MaximumLength(100)
                .WithMessage("Membership name must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrWhiteSpace(x.Description))
                .WithMessage("Description must not exceed 1000 characters.");

            RuleFor(x => x.DurationDays)
                .GreaterThan(0)
                .WithMessage("Duration must be greater than 0 days.");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Price cannot be negative.");
        }
    }
}
