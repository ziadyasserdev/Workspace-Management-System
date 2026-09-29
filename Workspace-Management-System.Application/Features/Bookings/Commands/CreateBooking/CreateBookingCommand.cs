using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.Bookings.Commands.CreateBooking
{
    public class CreateBookingCommand : IRequest<Result<int>>
    {
        public int CustomerId { get; set; }
        public int WorkspaceId { get; set; }

        public DateTime BookingDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? ExpectedEndTime { get; set; }

        public int NumberOfPeople { get; set; }
        public string? Notes { get; set; }
    }
}
