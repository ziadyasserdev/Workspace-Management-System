using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommand : IRequest<Result<int>>
    {
        public string FullName { get; set; }

        public string MobileNumber { get; set; }

        public string? Email { get; set; }

        public int? CompanyId { get; set; }

        public CustomerType CustomerType { get; set; }

        public string? Notes { get; set; }
    }
}
