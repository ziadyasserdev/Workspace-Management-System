using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Bookings.Commands.CreateBooking
{

    public class CreateBookingCommandValidator
        : AbstractValidator<CreateBookingCommand>
    {
        public CreateBookingCommandValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.CustomerId)
                .GreaterThan(0)
                .WithMessage(
                    localizer["CustomerIdMustBeGreaterThanZero"].Value);

            RuleFor(x => x.WorkspaceId)
                .GreaterThan(0)
                .WithMessage(
                    localizer["WorkspaceIdMustBeGreaterThanZero"].Value);

            RuleFor(x => x.BookingDate)
                .NotEmpty()
                .WithMessage(
                    localizer["BookingDateRequired"].Value);

            RuleFor(x => x.StartTime)
                .NotEmpty()
                .WithMessage(
                    localizer["BookingStartTimeRequired"].Value);

            RuleFor(x => x.ExpectedEndTime)
                .GreaterThan(x => x.StartTime)
                .When(x => x.ExpectedEndTime.HasValue)
                .WithMessage(
                    localizer["ExpectedEndTimeAfterStartTime"].Value);

            RuleFor(x => x.NumberOfPeople)
                .GreaterThan(0)
                .WithMessage(
                    localizer["NumberOfPeopleMustBeGreaterThanZero"].Value);

            RuleFor(x => x.Notes)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrWhiteSpace(x.Notes))
                .WithMessage(
                    localizer["BookingNotesMaxLength"].Value);
        }
    }
}
