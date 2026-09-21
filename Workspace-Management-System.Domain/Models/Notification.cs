using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Domain.Enums;
using Workspace_Management_System.Domain.Identity;

namespace Workspace_Management_System.Domain.Models
{
    public class Notification : BaseEntity
    {
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }

        public NotificationType Type { get; set; }
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;

        public bool IsRead { get; set; }

        public ApplicationUser Sender { get; set; } = null!;
        public ApplicationUser Receiver { get; set; } = null!;
    }
}
