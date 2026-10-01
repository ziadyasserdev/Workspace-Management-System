using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Memberships.Dtos
{
    public class MembershipBenefitDto
    {
        public int Id { get; set; }

        public BenefitType BenefitType { get; set; }

        public int? WorkspaceTypeId { get; set; }

        public string? WorkspaceTypeName { get; set; }

        public decimal? Hours { get; set; }

        public decimal? DiscountPercentage { get; set; }

        public string? Description { get; set; }
    }
    public class MembershipDetailsDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int DurationDays { get; set; }

        public decimal Price { get; set; }

        public bool IsActive { get; set; }

        public List<MembershipBenefitDto> Benefits { get; set; }
            = new();
    }
}
