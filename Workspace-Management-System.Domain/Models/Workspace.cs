using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Domain.Enums;
using static System.Collections.Specialized.BitVector32;

namespace Workspace_Management_System.Domain.Models
{
    public class Workspace : BaseEntity
    {
        public int WorkspaceTypeId { get; set; }

        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string? Floor { get; set; }
        public string? Location { get; set; }
        public int Capacity { get; set; }
        public WorkspaceStatus Status { get; set; } 
        public string? Description { get; set; }
        public bool IsActive { get; set; }

        public WorkspaceType WorkspaceType { get; set; } = null!;

        public ICollection<Booking> Bookings { get; set; }
            = new List<Booking>();

        public ICollection<Session> Sessions { get; set; }
            = new List<Session>();
    }
}
