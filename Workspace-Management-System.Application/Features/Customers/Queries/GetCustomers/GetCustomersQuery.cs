using MediatR;
using Workspace_Management_System.Application.Common.PaginatedResults;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Features.Customers.Dtos;
namespace Workspace_Management_System.Application.Features.Customers.Queries.GetCustomers
{
    public class GetCustomersQuery
        : IRequest<Result<PaginatedResult<CustomerDto>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}