using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Domain.Models
{
    public class Package : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public PackageType PackageType { get; set; } 
        public decimal TotalHours { get; set; }
        public int? DurationDays { get; set; }

        public decimal Price { get; set; }
        public bool IsActive { get; set; }

        public ICollection<CustomerPackage> CustomerPackages { get; set; }
            = new List<CustomerPackage>();
    }
}
