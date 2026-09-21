using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Domain.Models
{

    public class SessionWorkspaceHistory : BaseEntity
    {
        public int SessionId { get; set; }

        public int WorkspaceId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int EmployeeId { get; set; }

        public string? Reason { get; set; }


        // Navigation Properties

        public Session Session { get; set; } = null!;

        public Workspace Workspace { get; set; } = null!;

        public Employee Employee { get; set; } = null!;
    }
}
