using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Workspace_Management_System.Domain.Models
{
    public class Transaction : BaseEntity
    {
        public string TransactionNumber { get; set; } = null!;

        public int SessionId { get; set; }
        public int CustomerId { get; set; }
        public int EmployeeId { get; set; }
        public int? DiscountId { get; set; }

        public decimal Subtotal { get; set; }
    public decimal TaxAmount { get; set; }
        public decimal Total { get; set; }

        public string Status { get; set; } = null!;

        public Session Session { get; set; } = null!;
        public Customer Customer { get; set; } = null!;
        public Employee Employee { get; set; } = null!;
        public Discount? Discount { get; set; }

        public ICollection<TransactionItem> Items { get; set; }
            = new List<TransactionItem>();

        public ICollection<Payment> Payments { get; set; }
            = new List<Payment>();

        public Invoice? Invoice { get; set; }
    }
}
