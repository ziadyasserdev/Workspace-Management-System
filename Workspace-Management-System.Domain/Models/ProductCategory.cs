using System.Collections.Generic;

namespace Workspace_Management_System.Domain.Models
{
    public class ProductCategory : BaseEntity
    {
        public string NameEn { get; set; } = null!;
        public string NameAr { get; set; } = null!;

        public string? DescriptionEn { get; set; }
        public string? DescriptionAr { get; set; }

        public bool IsActive { get; set; }

        public ICollection<Product> Products { get; set; }
            = new List<Product>();
    }
}