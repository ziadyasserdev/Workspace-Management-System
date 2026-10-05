using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.CustomerMemberships.Dtos
{
    public class CustomerMembershipDto
    {
        public int Id { get; set; }

        public int MembershipId { get; set; }

        public string MembershipName { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public CustomerMembershipStatus Status { get; set; }

        public decimal PricePaid { get; set; }
    }
}
