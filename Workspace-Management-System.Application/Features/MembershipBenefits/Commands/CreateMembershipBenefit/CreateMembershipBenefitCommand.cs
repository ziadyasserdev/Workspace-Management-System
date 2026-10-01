using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.MembershipBenefits.Commands.CreateMembershipBenefit
{
    public class CreateMembershipBenefitCommand
       : IRequest<Result<int>>
    {
        public int MembershipId { get; set; }

        public BenefitType BenefitType { get; set; }

        public int? WorkspaceTypeId { get; set; }

        public decimal? Hours { get; set; }

        public decimal? DiscountPercentage { get; set; }

        public string? Description { get; set; }
    }
}
