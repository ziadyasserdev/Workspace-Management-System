using MediatR;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Features.Customers.Dtos;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Customers.Queries.GetCustomerTypes
{
    public class GetCustomerTypesQueryHandler
        : IRequestHandler<
            GetCustomerTypesQuery,
            Result<List<CustomerTypeDto>>>
    {
        public Task<Result<List<CustomerTypeDto>>> Handle(
            GetCustomerTypesQuery request,
            CancellationToken cancellationToken)
        {
            var customerTypes = Enum.GetValues<CustomerType>()
                .Select(type => new CustomerTypeDto
                {
                    Value = (int)type,
                    Name = type.ToString()
                })
                .ToList();

            return Task.FromResult(
                Result<List<CustomerTypeDto>>.Success(
                    customerTypes,
                    "Customer types retrieved successfully."));
        }
    }
}