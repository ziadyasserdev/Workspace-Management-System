using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.Customers.Commands.Delete_Customer
{
    public class DeleteCustomerCommand : IRequest<Result<bool>>
    {
        public int Id { get; set; }
    }
}
