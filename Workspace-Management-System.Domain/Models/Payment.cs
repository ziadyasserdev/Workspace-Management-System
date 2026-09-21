using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Domain.Models
{
    public class Payment : BaseEntity
    {
        public int TransactionId { get; set; }

        public PaymentMethod PaymentMethod { get; set; } 
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }

        public PaymentStatus Status { get; set; } 
        public string? ReferenceNumber { get; set; }

        public Transaction Transaction { get; set; } = null!;
    }
}
