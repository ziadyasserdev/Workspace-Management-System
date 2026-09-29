using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;

namespace Workspace_Management_System.Application.Features.Discounts.Commands.DeleteDiscount
{
    public class DeleteDiscountCommandHandler
        : IRequestHandler<DeleteDiscountCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public DeleteDiscountCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<bool>> Handle(
            DeleteDiscountCommand request,
            CancellationToken cancellationToken)
        {
            var discount = await _unitOfWork.Discounts
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.IsDeleted,
                    cancellationToken);

            if (discount is null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    "Discount not found.");
            }

            discount.IsDeleted = true;
            discount.IsActive = false;
            discount.IsDeletedBy = _currentUser.UserId;
            discount.UpdatedAt = DateTime.UtcNow;
            discount.UpdatedBy = _currentUser.UserId;

            _unitOfWork.Discounts.Update(discount);

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                "Discount deleted successfully.");
        }
    }
}