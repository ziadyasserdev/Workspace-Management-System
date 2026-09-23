using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Domain.Models
{
    public class Session : BaseEntity
    {
        public int CustomerId { get; set; }
        public int? BookingId { get; set; }
        public int WorkspaceId { get; set; }
        public int PricingPlanId { get; set; }
        public int EmployeeId { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public int NumberOfPeople { get; set; }
        public SessionStatus Status { get; set; } 
        public Customer Customer { get; set; } = null!;
        public Booking? Booking { get; set; }
        public Workspace Workspace { get; set; } = null!;
        public PricingPlan PricingPlan { get; set; } = null!;
        public Employee Employee { get; set; } = null!;

        public ICollection<SessionWorkspaceHistory> WorkspaceHistory { get; set; }
            = new List<SessionWorkspaceHistory>();

        public ICollection<Transaction> Transactions { get; set; }
            = new List<Transaction>();
    }
}
