using MediatR;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Features.Customers.Dtos;

namespace Workspace_Management_System.Application.Features.Customers.Queries.GetCustomerById
{
    public class GetCustomerByIdQuery
        : IRequest<Result<CustomerDto>>
    {
        public int Id { get; set; }
    }
}