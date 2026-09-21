using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Domain.Models
{
    public class CustomerPackage : BaseEntity
    {
        public int CustomerId { get; set; }
        public int PackageId { get; set; }

        public DateTime PurchaseDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public decimal? RemainingHours { get; set; }
        public CustomerPackageStatus Status { get; set; } 

        public Customer Customer { get; set; } = null!;
        public Package Package { get; set; } = null!;
    }
}
