using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Domain.Models
{
    public class Product : BaseEntity
    {
        public int ProductCategoryId { get; set; }

        public string EnglishName { get; set; } = null!;
        public string? Description { get; set; }
        public string Sku { get; set; } = null!;

        public decimal SellingPrice { get; set; }
        public decimal CostPrice { get; set; }


        public bool IsActive { get; set; }

        public ProductCategory ProductCategory { get; set; } = null!;

        public Inventory Inventory { get; set; } = null!;

        public ICollection<StockMovement> StockMovements { get; set; }
            = new List<StockMovement>();

        public ICollection<TransactionItem> TransactionItems { get; set; }
            = new List<TransactionItem>();
    }
}
