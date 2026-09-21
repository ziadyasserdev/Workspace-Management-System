using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Domain.Identity;

namespace Workspace_Management_System.Domain.Models
{
    public class AuditLog : BaseEntity
    {
        public string UserId { get; set; }

        public string Action { get; set; } = null!;
        public string EntityName { get; set; } = null!;
        public int EntityId { get; set; }

        public string? OldValues { get; set; }
        public string? NewValues { get; set; }

        public string? Reason { get; set; }
        public string? IpAddress { get; set; }

        public ApplicationUser User { get; set; } = null!;
    }
}
