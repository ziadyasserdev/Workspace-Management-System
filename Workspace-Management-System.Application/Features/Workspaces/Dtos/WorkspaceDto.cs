using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Workspaces.Dtos
{
    public class WorkspaceDto
    {
        public int Id { get; set; }

        public int WorkspaceTypeId { get; set; }

        public string WorkspaceTypeName { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Code { get; set; } = null!;

        public string? Floor { get; set; }

        public string? Location { get; set; }

        public int Capacity { get; set; }

        public WorkspaceStatus Status { get; set; }

        public string? Description { get; set; }

        public bool IsActive { get; set; }
    }
}
