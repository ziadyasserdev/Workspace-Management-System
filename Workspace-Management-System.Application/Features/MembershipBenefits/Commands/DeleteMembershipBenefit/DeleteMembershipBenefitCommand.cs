using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.MembershipBenefits.Commands.DeleteMembershipBenefit
{
    public class DeleteMembershipBenefitCommand
     : IRequest<Result<bool>>
    {
        public int MembershipId { get; set; }

        public int BenefitId { get; set; }
    }
}
