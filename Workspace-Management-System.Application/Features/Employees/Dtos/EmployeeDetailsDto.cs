using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Employees.Dtos
{
    public class EmployeeDetailsDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string FullName { get; set; } = null!;

        public string Phone { get; set; } = null!;
        public string? Email { get; set; }

        public DateTime HireDate { get; set; }
        public EmployeeStatus Status { get; set; }

        public int? WorkspaceId { get; set; }
        public string? WorkspaceName { get; set; }
        public string? WorkspaceCode { get; set; }
    }
}
