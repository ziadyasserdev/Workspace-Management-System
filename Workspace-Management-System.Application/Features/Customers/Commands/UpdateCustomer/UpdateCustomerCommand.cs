using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.Customers.Commands.UpdateCustomer
{
        public class UpdateCustomerCommand : IRequest<Result<bool>>
        {
            public int Id { get; set; }

            public string FullName { get; set; } = null!;

            public string MobileNumber { get; set; } = null!;

            public string? Email { get; set; }

            public int? CompanyId { get; set; }

            public string CustomerType { get; set; } = null!;

            public string? Notes { get; set; }
       
    }
}
