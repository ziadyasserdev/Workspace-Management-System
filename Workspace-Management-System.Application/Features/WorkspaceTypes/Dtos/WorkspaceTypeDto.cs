using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspace_Management_System.Application.Features.WorkspaceTypes.Dtos
{
    public class WorkspaceTypeDto
    {
        public int Id { get; set; }

        public string NameEn { get; set; } = null!;

        public string NameAr { get; set; } = null!;

        public string? DescriptionEn { get; set; }

        public string? DescriptionAr { get; set; }

        public bool IsActive { get; set; }
    }
}
