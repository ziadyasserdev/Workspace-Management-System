using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Domain.Models
{
    public class ExpenseCategory : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Expense> Expenses { get; set; }
            = new List<Expense>();
    }
}
