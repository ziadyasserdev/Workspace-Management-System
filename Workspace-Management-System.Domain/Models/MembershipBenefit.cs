using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Domain.Models
{
    public class MembershipBenefit : BaseEntity
    {
        public int MembershipId { get; set; }

        public BenefitType BenefitType { get; set; } 
        public int? WorkspaceTypeId { get; set; }

        public decimal? Hours { get; set; }
        public decimal? DiscountPercentage { get; set; }

        public string? Description { get; set; }

        public Membership Membership { get; set; } = null!;
        public WorkspaceType? WorkspaceType { get; set; }
    }
}
