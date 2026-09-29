using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Application.Features.Bookings.Queries.GetBookingById
{
    public class GetBookingByIdQueryValidator
       : AbstractValidator<GetBookingByIdQuery>
    {
        public GetBookingByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Booking id must be greater than 0.");
        }
    }
}
