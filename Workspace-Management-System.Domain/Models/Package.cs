using System;
using System.Collections.Generic;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Domain.Models
{
    public class Package : BaseEntity
    {
        public string NameEn { get; set; } = null!;
        public string NameAr { get; set; } = null!;

        public string? DescriptionEn { get; set; }
        public string? DescriptionAr { get; set; }

        public PackageType PackageType { get; set; }
        public decimal TotalHours { get; set; }
        public int? DurationDays { get; set; }

        public decimal Price { get; set; }
        public bool IsActive { get; set; }

        public ICollection<CustomerPackage> CustomerPackages { get; set; }
            = new List<CustomerPackage>();
    }
}