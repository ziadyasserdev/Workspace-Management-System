using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Bookings.Commands.ChangeBookingStatus
{
     public class ChangeBookingStatusCommandValidator
        : AbstractValidator<ChangeBookingStatusCommand>
    {
        public ChangeBookingStatusCommandValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(
                    localizer["BookingIdMustBeGreaterThanZero"].Value);

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage(
                    localizer["InvalidBookingStatus"].Value);
        }
    }
}
