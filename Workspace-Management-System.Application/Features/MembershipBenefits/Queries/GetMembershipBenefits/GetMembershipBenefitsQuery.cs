using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Features.MembershipBenefits.Dtos;

namespace Workspace_Management_System.Application.Features.MembershipBenefits.Queries.GetMembershipBenefits
{
    public class GetMembershipBenefitsQuery
     : IRequest<Result<List<MembershipBenefitDtoo>>>
    {
        public int MembershipId { get; set; }
    }
}
