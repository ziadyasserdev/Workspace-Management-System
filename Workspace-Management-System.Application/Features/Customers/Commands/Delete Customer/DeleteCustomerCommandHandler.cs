using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Customers.Commands.Delete_Customer;

namespace Workspace_Management_System.Application.Features.Customers.Commands.DeleteCustomer
{
    public class DeleteCustomerCommandHandler
        : IRequestHandler<DeleteCustomerCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public DeleteCustomerCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<bool>> Handle(
            DeleteCustomerCommand request,
            CancellationToken cancellationToken)
        {
            var customer = await _unitOfWork.Customers
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id,
                    cancellationToken);

            if (customer is null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    "Customer not found.");
            }

            if (customer.IsDeleted)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "Customer is already deleted.");
            }

            customer.IsDeleted = true;

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(true);
        }
    }
}