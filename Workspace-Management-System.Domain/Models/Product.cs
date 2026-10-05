using System.Collections.Generic;

namespace Workspace_Management_System.Domain.Models
{
    public class Product : BaseEntity
    {
        public int ProductCategoryId { get; set; }

        public string NameEn { get; set; } = null!;
        public string NameAr { get; set; } = null!;

        public string? DescriptionEn { get; set; }
        public string? DescriptionAr { get; set; }

        public string Sku { get; set; } = null!;

        public decimal SellingPrice { get; set; }
        public decimal CostPrice { get; set; }

        public bool IsActive { get; set; }

        public ProductCategory ProductCategory { get; set; } = null!;

        public Inventory Inventory { get; set; } = null!;

        public ICollection<StockMovement> StockMovements { get; set; }
            = new List<StockMovement>();

        public ICollection<SessionProduct> SessionProducts { get; set; }
            = new List<SessionProduct>();

        public ICollection<TransactionItem> TransactionItems { get; set; }
            = new List<TransactionItem>();
    }
}