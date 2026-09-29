using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Application.Features.Bookings.Queries.GetBookings
{
    public class GetBookingsQueryValidator
     : AbstractValidator<GetBookingsQuery>
    {
        public GetBookingsQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than 0.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("Page size must be between 1 and 100.");

            RuleFor(x => x.Search)
                .MaximumLength(100)
                .When(x => !string.IsNullOrWhiteSpace(x.Search))
                .WithMessage("Search must not exceed 100 characters.");

            RuleFor(x => x.Status)
                .IsInEnum()
                .When(x => x.Status.HasValue)
                .WithMessage("Invalid booking status.");

            RuleFor(x => x.WorkspaceId)
                .GreaterThan(0)
                .When(x => x.WorkspaceId.HasValue)
                .WithMessage("Workspace id must be greater than 0.");

            RuleFor(x => x.CustomerId)
                .GreaterThan(0)
                .When(x => x.CustomerId.HasValue)
                .WithMessage("Customer id must be greater than 0.");

            RuleFor(x => x.ToDate)
                .GreaterThanOrEqualTo(x => x.FromDate)
                .When(x =>
                    x.FromDate.HasValue &&
                    x.ToDate.HasValue)
                .WithMessage("To date must be greater than or equal to from date.");
        }
    }
}
