using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Features.Memberships.Dtos;

namespace Workspace_Management_System.Application.Features.Memberships.Queries.GetMembershipById
{
    public class GetMembershipByIdQuery
    : IRequest<Result<MembershipDetailsDto>>
    {
        public int Id { get; set; }
    }
}
