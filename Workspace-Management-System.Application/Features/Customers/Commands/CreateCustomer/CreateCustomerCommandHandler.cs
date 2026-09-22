using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Domain.Enums;
using Workspace_Management_System.Domain.Models;

namespace Workspace_Management_System.Application.Features.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommandHandler: IRequestHandler<CreateCustomerCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;

        public CreateCustomerCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(
            CreateCustomerCommand request,
            CancellationToken cancellationToken)
        {
            var checkExist = await unitOfWork.Customers
                .Query()
                .AnyAsync(
                    x => x.MobileNumber == request.MobileNumber && !x.IsDeleted,
                    cancellationToken);
            if (checkExist)
            {
                return Result<int>.Failure(
                    ResultStatus.Conflict,
                    "A customer with the same mobile number already exists.");
            }
            var customer = new Customer
            {
                FullName = request.FullName,
                MobileNumber = request.MobileNumber,
                Email = request.Email,
                CompanyId = request.CompanyId,
                CustomerType = request.CustomerType,
                Notes = request.Notes,
                RegistrationDate = DateTime.UtcNow,
                Status = CustomerStatus.Active
            };

            await unitOfWork.Customers.AddAsync(customer);

            await unitOfWork.SaveAsync();

            return Result<int>.Success(customer.Id,"Customer created successfully.");
        }
    }
}