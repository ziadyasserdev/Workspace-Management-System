using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Domain.Models
{
    public class Company : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string ContactPerson { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string? Email { get; set; }
        public string? TaxNumber { get; set; }
        public string? TaxInformation { get; set; }
        public string? ContractDetails { get; set; }

        public int? PricingPlanId { get; set; }
        public decimal CreditLimit { get; set; }
        public bool IsActive { get; set; }

        public PricingPlan? PricingPlan { get; set; }

        public ICollection<Customer> Customers { get; set; }
            = new List<Customer>();
    }
}
