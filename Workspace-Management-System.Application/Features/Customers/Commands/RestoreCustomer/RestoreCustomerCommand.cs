using MediatR;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.Customers.Commands.RestoreCustomer
{
    public class RestoreCustomerCommand : IRequest<Result<bool>>
    {
        public int Id { get; set; }
    }
}