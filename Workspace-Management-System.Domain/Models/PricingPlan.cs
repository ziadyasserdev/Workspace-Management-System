using System.Collections.Generic;

namespace Workspace_Management_System.Domain.Models
{
    public class PricingPlan : BaseEntity
    {
        public string NameEn { get; set; } = null!;
        public string NameAr { get; set; } = null!;

        public string? DescriptionEn { get; set; }
        public string? DescriptionAr { get; set; }

        public bool IsActive { get; set; }

        public ICollection<PricingRule> PricingRules { get; set; }
            = new List<PricingRule>();

        public ICollection<Company> Companies { get; set; }
            = new List<Company>();

        public ICollection<Session> Sessions { get; set; }
            = new List<Session>();
    }
}