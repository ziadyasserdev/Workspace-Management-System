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
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string FullName  => $"{FirstName} {LastName}";

        public string Phone { get; set; } = null!;

        public string? Email { get; set; }

        public Gender Gender { get; set; }
        public DateTime HireDate { get; set; }

        public EmployeeStatus Status { get; set; }

        // Workspace
        public int? WorkspaceId { get; set; }

        // Identity User
        public string? UserId { get; set; }

        public Workspace Workspace { get; set; } = null!;

        public ApplicationUser? User { get; set; }

        public ICollection<Session> Sessions { get; set; }
            = new List<Session>();

        public ICollection<Transaction> Transactions { get; set; }
            = new List<Transaction>();

        public ICollection<Expense> Expenses { get; set; }
            = new List<Expense>();
    }
}
