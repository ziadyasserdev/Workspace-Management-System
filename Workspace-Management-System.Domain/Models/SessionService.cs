using System;

namespace Workspace_Management_System.Domain.Models
{
    public class SessionService : BaseEntity
    {
        public int SessionId { get; set; }
        public int ServiceId { get; set; }

        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public Session Session { get; set; } = null!;
        public Service Service { get; set; } = null!;
    }
}