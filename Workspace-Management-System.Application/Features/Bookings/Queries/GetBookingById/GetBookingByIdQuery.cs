using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Features.Bookings.Dtos;

namespace Workspace_Management_System.Application.Features.Bookings.Queries.GetBookingById
{
    public class GetBookingByIdQuery : IRequest<Result<BookingDetailsDto>>
    {
        public int Id { get; set; }
    }
}
