using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Domain.Models
{
    public class Membership : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public int DurationDays { get; set; }
        public decimal Price { get; set; }

        public bool IsActive { get; set; }

        public ICollection<MembershipBenefit> Benefits { get; set; }
            = new List<MembershipBenefit>();

        public ICollection<CustomerMembership> CustomerMemberships { get; set; }
            = new List<CustomerMembership>();
    }
}
