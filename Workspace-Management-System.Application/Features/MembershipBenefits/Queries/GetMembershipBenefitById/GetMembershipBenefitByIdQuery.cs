using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Features.Memberships.Dtos;

namespace Workspace_Management_System.Application.Features.MembershipBenefits.Queries.GetMembershipBenefitById
{
    public class GetMembershipBenefitByIdQuery
      : IRequest<Result<MembershipBenefitDto>>
    {
        public int MembershipId { get; set; }

        public int BenefitId { get; set; }
    }
}
