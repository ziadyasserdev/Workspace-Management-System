using System.Collections.Generic;

namespace Workspace_Management_System.Domain.Models
{
    public class Company : BaseEntity
    {
        public string NameEn { get; set; } = null!;
        public string NameAr { get; set; } = null!;

        public string ContactPerson { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string? Email { get; set; }
        public string? TaxNumber { get; set; }

        public string? TaxInformationEn { get; set; }
        public string? TaxInformationAr { get; set; }

        public string? ContractDetailsEn { get; set; }
        public string? ContractDetailsAr { get; set; }

        public int? PricingPlanId { get; set; }
        public decimal CreditLimit { get; set; }
        public bool IsActive { get; set; }

        public PricingPlan? PricingPlan { get; set; }

        public ICollection<Customer> Customers { get; set; }
            = new List<Customer>();
    }
}