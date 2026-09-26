using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;

namespace Workspace_Management_System.Application.Features.Customers.Commands.RestoreCustomer
{
    public class RestoreCustomerCommandHandler
        : IRequestHandler<RestoreCustomerCommand, Result<bool>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUser;

        public RestoreCustomerCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            this.unitOfWork = unitOfWork;
            this.currentUser = currentUser;
        }

        public async Task<Result<bool>> Handle(
            RestoreCustomerCommand request,
            CancellationToken cancellationToken)
        {
            var customer = await unitOfWork.Customers
                .Query()
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id,
                    cancellationToken);

            if (customer == null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    "Customer not found.");
            }

            if (!customer.IsDeleted)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "Customer is already active.");
            }

            var mobileExists = await unitOfWork.Customers
                .Query()
                .AnyAsync(
                    x => x.Id != customer.Id &&
                         x.MobileNumber == customer.MobileNumber &&
                         !x.IsDeleted,
                    cancellationToken);

            if (mobileExists)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "Another active customer already uses this mobile number.");
            }

            customer.IsDeleted = false;
            customer.IsDeletedBy = null;
            customer.UpdatedAt = DateTime.UtcNow;
            customer.UpdatedBy = currentUser.UserId;

            await unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                "Customer restored successfully.");
        }
    }
}