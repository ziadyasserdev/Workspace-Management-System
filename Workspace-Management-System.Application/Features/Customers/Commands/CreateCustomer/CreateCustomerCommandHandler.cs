using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Domain.Enums;
using Workspace_Management_System.Domain.Models;

namespace Workspace_Management_System.Application.Features.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommandHandler
        : IRequestHandler<CreateCustomerCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUser;

        public CreateCustomerCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            this.unitOfWork = unitOfWork;
            this.currentUser = currentUser;
        }

        public async Task<Result<int>> Handle(
            CreateCustomerCommand request,
            CancellationToken cancellationToken)
        {
            var checkExist = await unitOfWork.Customers
                .Query()
                .AnyAsync(
                    x =>
                        x.MobileNumber == request.MobileNumber &&
                        !x.IsDeleted,
                    cancellationToken);

            if (checkExist)
            {
                return Result<int>.Failure(
                    ResultStatus.Conflict,
                    "A customer with the same mobile number already exists.");
            }

            var now = DateTime.UtcNow;

            var customer = new Customer
            {
                FullNameEn = request.FullNameEn.Trim(),
                FullNameAr = request.FullNameAr.Trim(),
                MobileNumber = request.MobileNumber,
                Email = request.Email,
                CompanyId = request.CompanyId,
                CustomerType = request.CustomerType,
                NotesEn = request.NotesEn?.Trim(),
                NotesAr = request.NotesAr?.Trim(),
                RegistrationDate = now,
                Status = CustomerStatus.Active,
                CreatedAt = now,
                CreatedBy = currentUser.UserId
            };

            await unitOfWork.Customers.AddAsync(customer);

            await unitOfWork.SaveAsync();

            return Result<int>.Success(
                customer.Id,
                "Customer created successfully.");
        }
    }
}