using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Features.CustomerMemberships.Dtos;

namespace Workspace_Management_System.Application.Features.CustomerMemberships.Queries.GetCustomerMemberships
{
    public class GetCustomerMembershipsQuery
     : IRequest<Result<List<CustomerMembershipDto>>>
    {
        public int CustomerId { get; set; }
    }
}
