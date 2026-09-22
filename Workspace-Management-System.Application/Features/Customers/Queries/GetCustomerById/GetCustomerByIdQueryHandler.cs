using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Customers.Dtos;

namespace Workspace_Management_System.Application.Features.Customers.Queries.GetCustomerById
{
    public class GetCustomerByIdQueryHandler
        : IRequestHandler<
            GetCustomerByIdQuery,
            Result<CustomerDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCustomerByIdQueryHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CustomerDto>> Handle(
            GetCustomerByIdQuery request,
            CancellationToken cancellationToken)
        {
            var customer = await _unitOfWork.Customers
                .Query()
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id,
                    cancellationToken);

            if (customer is null)
            {
                return Result<CustomerDto>.Failure(
                    ResultStatus.NotFound,
                    "Customer not found.");
            }

            var result = new CustomerDto
            {
                Id = customer.Id,
                FullName = customer.FullName,
                MobileNumber = customer.MobileNumber,
                Email = customer.Email,
                CompanyId = customer.CompanyId,
                CustomerType = customer.CustomerType,
                Notes = customer.Notes,
                RegistrationDate = customer.RegistrationDate,
                Status = customer.Status
            };

            return Result<CustomerDto>.Success(
                result,
                "Customer retrieved successfully.");
        }
    }
}