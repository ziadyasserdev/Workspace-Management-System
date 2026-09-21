using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Workspace_Management_System.Domain.Models
{
    public class PricingPlan : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }

        public ICollection<PricingRule> PricingRules { get; set; }
            = new List<PricingRule>();

        public ICollection<Company> Companies { get; set; }
            = new List<Company>();

        public ICollection<Session> Sessions { get; set; }
            = new List<Session>();
    }
}
