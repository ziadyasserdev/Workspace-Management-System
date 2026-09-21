using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Domain.Models
{
    public class Invoice : BaseEntity
    {
        public string InvoiceNumber { get; set; } = null!;

        public int TransactionId { get; set; }
        public int CustomerId { get; set; }

        public DateTime InvoiceDate { get; set; }

        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }

        public string Status { get; set; } = null!;

        public Transaction Transaction { get; set; } = null!;
        public Customer Customer { get; set; } = null!;
    }
}
