using MediatR;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Features.Customers.Dtos;

namespace Workspace_Management_System.Application.Features.Customers.Queries.GetCustomerTypes
{
    public class GetCustomerTypesQuery
        : IRequest<Result<List<CustomerTypeDto>>>
    {
    }
}