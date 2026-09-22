using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;

namespace Workspace_Management_System.Application.Features.Customers.Commands.ChangeCustomerStatus
{
    public class ChangeCustomerStatusCommandHandler
        : IRequestHandler<
            ChangeCustomerStatusCommand,
            Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChangeCustomerStatusCommandHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(
            ChangeCustomerStatusCommand request,
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
                    "Customer is deleted.");
            }

            customer.Status = request.Status;

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                "Customer status changed successfully.");
        }
    }
}