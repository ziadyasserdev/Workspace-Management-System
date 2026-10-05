using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.PaginatedResults;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Features.Memberships.Dtos;

namespace Workspace_Management_System.Application.Features.Memberships.Queries.GetMemberships
{
    public class GetMembershipsQuery
        : IRequest<Result<PaginatedResult<MembershipListDto>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string? Search { get; set; }
        public bool? IsActive { get; set; }

        public bool SortDescending { get; set; } = false;
    }
}
