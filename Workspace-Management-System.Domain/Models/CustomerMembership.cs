using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Domain.Models
{
    public class CustomerMembership : BaseEntity
    {
        public int CustomerId { get; set; }
        public int MembershipId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public CustomerMembershipStatus Status { get; set; } 
        public decimal PricePaid { get; set; }

        public Customer Customer { get; set; } = null!;
        public Membership Membership { get; set; } = null!;
    }
}
