using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Domain.Models
{
    public class Discount : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public DiscountType DiscountType { get; set; } 
        public decimal Value { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
            = new List<Transaction>();

        public ICollection<TransactionItem> TransactionItems { get; set; }
            = new List<TransactionItem>();
    }
}
