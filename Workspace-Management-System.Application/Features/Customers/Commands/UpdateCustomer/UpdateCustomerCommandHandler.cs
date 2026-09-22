using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Domain.Models;

namespace Workspace_Management_System.Application.Features.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommandHandler: IRequestHandler<UpdateCustomerCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public UpdateCustomerCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<bool>> Handle(
            UpdateCustomerCommand request,
            CancellationToken cancellationToken)
        {
            var customer = await _unitOfWork.Customers
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id &&
                         !x.IsDeleted,
                    cancellationToken);

            if (customer is null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    "Customer not found.");
            }

            var exists = await _unitOfWork.Customers
                .Query()
                .AnyAsync(
                    x => x.Id != request.Id &&
                         x.MobileNumber == request.MobileNumber &&
                         !x.IsDeleted,
                    cancellationToken);

            if (exists)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "A customer with the same mobile number already exists.");
            }

            customer.FullName = request.FullName;
            customer.MobileNumber = request.MobileNumber;
            customer.Email = request.Email;
            customer.CompanyId = request.CompanyId;
            customer.CustomerType = request.CustomerType;
            customer.Notes = request.Notes;
            await _unitOfWork.SaveAsync();
            return Result<bool>.Success(true);
        }
    }
}