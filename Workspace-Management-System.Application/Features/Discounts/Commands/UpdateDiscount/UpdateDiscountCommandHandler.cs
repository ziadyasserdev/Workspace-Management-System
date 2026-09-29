using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;

namespace Workspace_Management_System.Application.Features.Discounts.Commands.UpdateDiscount
{
    public class UpdateDiscountCommandHandler
        : IRequestHandler<UpdateDiscountCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public UpdateDiscountCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<bool>> Handle(
            UpdateDiscountCommand request,
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

            var name = request.Name.Trim();

            var duplicate = await _unitOfWork.Discounts
                .Query()
                .AnyAsync(
                    x =>
                        x.Id != request.Id &&
                        x.Name == name &&
                        !x.IsDeleted,
                    cancellationToken);

            if (duplicate)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "A discount with the same name already exists.");
            }

            discount.Name = name;
            discount.DiscountType = request.Type;
            discount.Value = request.Value;
            discount.IsActive = request.IsActive;
            discount.StartDate = request.StartDate;
            discount.EndDate = request.EndDate;

            discount.UpdatedAt = DateTime.UtcNow;
            discount.UpdatedBy = _currentUser.UserId;

            _unitOfWork.Discounts.Update(discount);

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                "Discount updated successfully.");
        }
    }
}