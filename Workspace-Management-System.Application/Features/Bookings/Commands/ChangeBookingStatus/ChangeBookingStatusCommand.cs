using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Bookings.Commands.ChangeBookingStatus
{
    public class ChangeBookingStatusCommand
      : IRequest<Result<bool>>
    {
        public int Id { get; set; }

        public BookingStatus Status { get; set; }
    }
}
