using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Bookings.Dtos
{
    public class BookingListDto
    {
        public int Id { get; set; }

        public string BookingNumber { get; set; } = null!;

        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = null!;

        public int WorkspaceId { get; set; }
        public string WorkspaceName { get; set; } = null!;
        public string WorkspaceCode { get; set; } = null!;

        public DateTime BookingDate { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? ExpectedEndTime { get; set; }

        public int NumberOfPeople { get; set; }

        public BookingStatus Status { get; set; }

        public string? Notes { get; set; }

        public bool HasSession { get; set; }
    }
}
