using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Application.Features.Bookings.Commands.CreateBooking
{
    public class CreateBookingCommandValidator
     : AbstractValidator<CreateBookingCommand>
    {
        public CreateBookingCommandValidator()
        {
            RuleFor(x => x.CustomerId)
                .GreaterThan(0)
                .WithMessage("Customer id must be greater than 0.");

            RuleFor(x => x.WorkspaceId)
                .GreaterThan(0)
                .WithMessage("Workspace id must be greater than 0.");

            RuleFor(x => x.BookingDate)
                .NotEmpty()
                .WithMessage("Booking date is required.");

            RuleFor(x => x.StartTime)
                .NotEmpty()
                .WithMessage("Start time is required.");

            RuleFor(x => x.ExpectedEndTime)
                .GreaterThan(x => x.StartTime)
                .When(x => x.ExpectedEndTime.HasValue)
                .WithMessage("Expected end time must be after start time.");

            RuleFor(x => x.NumberOfPeople)
                .GreaterThan(0)
                .WithMessage("Number of people must be greater than 0.");

            RuleFor(x => x.Notes)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrWhiteSpace(x.Notes))
                .WithMessage("Notes must not exceed 1000 characters.");
        }
    }
}
