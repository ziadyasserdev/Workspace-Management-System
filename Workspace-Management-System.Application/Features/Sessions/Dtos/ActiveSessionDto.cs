using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Sessions.Dtos
{
    public class ActiveSessionDto
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = null!;

        public int WorkspaceId { get; set; }
        public string WorkspaceName { get; set; } = null!;
        public string WorkspaceCode { get; set; } = null!;

        public int PricingPlanId { get; set; }
        public string PricingPlanName { get; set; } = null!;

        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = null!;

        public DateTime StartTime { get; set; }
        public int NumberOfPeople { get; set; }

        public SessionStatus Status { get; set; }
    }
}
