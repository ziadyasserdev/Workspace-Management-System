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

        public string FirstNameEn { get; set; } = string.Empty;
        public string FirstNameAr { get; set; } = string.Empty;

        public string LastNameEn { get; set; } = string.Empty;
        public string LastNameAr { get; set; } = string.Empty;

        public string FullNameEn { get; set; } = string.Empty;
        public string FullNameAr { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;
        public string? Email { get; set; }

        public DateTime HireDate { get; set; }
        public EmployeeStatus Status { get; set; }

        public int? WorkspaceId { get; set; }

        public string? WorkspaceNameEn { get; set; }
        public string? WorkspaceNameAr { get; set; }

        public string? WorkspaceCode { get; set; }
    }
}
