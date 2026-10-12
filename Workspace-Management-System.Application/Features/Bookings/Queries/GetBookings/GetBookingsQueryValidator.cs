using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Bookings.Queries.GetBookings
{
    public class GetBookingsQueryValidator
        : AbstractValidator<GetBookingsQuery>
    {
        public GetBookingsQueryValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage(
                    localizer["PageNumberMustBeGreaterThanZero"].Value);

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage(
                    localizer["PageSizeMustBeBetween1And100"].Value);

            RuleFor(x => x.Search)
                .MaximumLength(100)
                .When(x => !string.IsNullOrWhiteSpace(x.Search))
                .WithMessage(
                    localizer["SearchMaxLength"].Value);

            RuleFor(x => x.Status)
                .IsInEnum()
                .When(x => x.Status.HasValue)
                .WithMessage(
                    localizer["InvalidBookingStatus"].Value);

            RuleFor(x => x.WorkspaceId)
                .GreaterThan(0)
                .When(x => x.WorkspaceId.HasValue)
                .WithMessage(
                    localizer["WorkspaceIdMustBeGreaterThanZero"].Value);

            RuleFor(x => x.CustomerId)
                .GreaterThan(0)
                .When(x => x.CustomerId.HasValue)
                .WithMessage(
                    localizer["CustomerIdMustBeGreaterThanZero"].Value);

            RuleFor(x => x.ToDate)
                .GreaterThanOrEqualTo(x => x.FromDate)
                .When(x =>
                    x.FromDate.HasValue &&
                    x.ToDate.HasValue)
                .WithMessage(
                    localizer["ToDateMustBeGreaterThanOrEqualToFromDate"].Value);
        }
    }
}