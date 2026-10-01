using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.MembershipBenefits.Dtos
{
    public class MembershipBenefitDtoo
    {
        public int Id { get; set; }

        public BenefitType BenefitType { get; set; }

        public int? WorkspaceTypeId { get; set; }

        public string? WorkspaceTypeName { get; set; }

        public decimal? Hours { get; set; }

        public decimal? DiscountPercentage { get; set; }

        public string? Description { get; set; }
    }
}
