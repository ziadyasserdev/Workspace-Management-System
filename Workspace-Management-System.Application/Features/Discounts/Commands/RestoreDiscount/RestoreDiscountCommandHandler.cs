using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;

namespace Workspace_Management_System.Application.Features.Discounts.Commands.RestoreDiscount
{
    public class RestoreDiscountCommandHandler
        : IRequestHandler<RestoreDiscountCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public RestoreDiscountCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<bool>> Handle(
            RestoreDiscountCommand request,
            CancellationToken cancellationToken)
        {
            var discount = await _unitOfWork.Discounts
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && x.IsDeleted,
                    cancellationToken);

            if (discount is null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    "Deleted discount not found.");
            }

            discount.IsDeleted = false;
            discount.IsDeletedBy = null;
            discount.IsActive = true;
            discount.UpdatedAt = DateTime.UtcNow;
            discount.UpdatedBy = _currentUser.UserId;

            _unitOfWork.Discounts.Update(discount);

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                "Discount restored successfully.");
        }
    }
}