using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Domain.Models
{
    public class WorkspaceType : BaseEntity
    {
        public string NameEn { get; set; } = null!;
        public string NameAr { get; set; } = null!;

        public string? DescriptionEn { get; set; }
        public string? DescriptionAr { get; set; }

        public bool IsActive { get; set; }

        public ICollection<Workspace> Workspaces { get; set; }
            = new List<Workspace>();

        public ICollection<PricingRule> PricingRules { get; set; }
            = new List<PricingRule>();

        public ICollection<MembershipBenefit> MembershipBenefits { get; set; }
            = new List<MembershipBenefit>();
    }
}
