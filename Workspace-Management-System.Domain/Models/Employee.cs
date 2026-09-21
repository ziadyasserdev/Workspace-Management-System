using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Domain.Enums;
using Workspace_Management_System.Domain.Identity;
using static System.Collections.Specialized.BitVector32;

namespace Workspace_Management_System.Domain.Models
{
    public class Employee : BaseEntity
    {
        public string EmployeeNumber { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string? Email { get; set; }

        public string Position { get; set; } = null!;

        public int? DepartmentId { get; set; }

        public DateTime HireDate { get; set; }
        public EmployeeStatus Status { get; set; } 

        public int? UserId { get; set; }

        public Department? Department { get; set; }
        public ApplicationUser? User { get; set; }

        public ICollection<Session> Sessions { get; set; }
            = new List<Session>();

        public ICollection<Transaction> Transactions { get; set; }
            = new List<Transaction>();

        public ICollection<Expense> Expenses { get; set; }
            = new List<Expense>();
    }
}
