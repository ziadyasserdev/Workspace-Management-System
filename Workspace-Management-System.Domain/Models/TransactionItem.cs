using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Domain.Models
{
    public class TransactionItem : BaseEntity
    {
        public int TransactionId { get; set; }

        public string ItemType { get; set; } = null!;

        public int? ProductId { get; set; }
        public int? ServiceId { get; set; }

        public string Description { get; set; } = null!;

        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public int? DiscountId { get; set; }

        public decimal Total { get; set; }

        public Transaction Transaction { get; set; } = null!;
        public Product? Product { get; set; }
        public Service? Service { get; set; }
        public Discount? Discount { get; set; }
    }
}
