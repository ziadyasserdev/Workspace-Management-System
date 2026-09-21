using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Domain.Enums;
using static System.Collections.Specialized.BitVector32;

namespace Workspace_Management_System.Domain.Models
{
    public class Booking : BaseEntity
    {
        public string BookingNumber { get; set; } = null!;

        public int CustomerId { get; set; }
        public int WorkspaceId { get; set; }

        public DateTime BookingDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? ExpectedEndTime { get; set; }

        public int NumberOfPeople { get; set; }
        public BookingStatus Status { get; set; } 
        public string? Notes { get; set; }

        public Customer Customer { get; set; } = null!;
        public Workspace Workspace { get; set; } = null!;

        public Session? Session { get; set; }
    }
}
