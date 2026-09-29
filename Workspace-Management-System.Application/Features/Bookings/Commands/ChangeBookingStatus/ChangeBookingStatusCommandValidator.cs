using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Application.Features.Bookings.Commands.ChangeBookingStatus
{
    public class ChangeBookingStatusCommandValidator
    : AbstractValidator<ChangeBookingStatusCommand>
    {
        public ChangeBookingStatusCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Booking id must be greater than 0.");

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Invalid booking status.");
        }
    }
}
