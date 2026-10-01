using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Application.Features.Memberships.Dtos
{
    public class MembershipListDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public int DurationDays { get; set; }
        public decimal Price { get; set; }

        public bool IsActive { get; set; }

        public int BenefitsCount { get; set; }
        public int ActiveCustomerMembershipsCount { get; set; }
    }
}
