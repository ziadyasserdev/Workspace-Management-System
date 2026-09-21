using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Domain.Models
{
    public class Expense : BaseEntity
    {
        public int ExpenseCategoryId { get; set; }

        public string Description { get; set; } = null!;
        public decimal Amount { get; set; }

        public int EmployeeId { get; set; }

        public DateTime ExpenseDate { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? Notes { get; set; }

        public ExpenseCategory ExpenseCategory { get; set; } = null!;
        public Employee Employee { get; set; } = null!;
    }
}
