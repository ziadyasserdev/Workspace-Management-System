using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.CustomerMemberships.Commands.SubscribeCustomer
{
    public class SubscribeCustomerCommand : IRequest<Result<int>>
    {
        public int CustomerId { get; set; }

        public int MembershipId { get; set; }
    }
}
