using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;

namespace Workspace_Management_System.Application.Features.ProductCategories.Commands.UpdateProductCategory
{
    public class UpdateProductCategoryCommandHandler
        : IRequestHandler<UpdateProductCategoryCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public UpdateProductCategoryCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<bool>> Handle(
            UpdateProductCategoryCommand request,
            CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.ProductCategories
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.IsDeleted,
                    cancellationToken);

            if (category == null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    "Product category not found.");
            }

            var nameEn = request.NameEn.Trim();
            var nameAr = request.NameAr.Trim();

            var duplicate = await _unitOfWork.ProductCategories
                .Query()
                .AnyAsync(
                    x =>
                        x.Id != request.Id &&
                        !x.IsDeleted &&
                        (
                            x.NameEn.ToLower() == nameEn.ToLower() ||
                            x.NameAr.ToLower() == nameAr.ToLower()
                        ),
                    cancellationToken);

            if (duplicate)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "Another product category with the same name already exists.");
            }

            category.NameEn = nameEn;
            category.NameAr = nameAr;
            category.DescriptionEn = request.DescriptionEn?.Trim();
            category.DescriptionAr = request.DescriptionAr?.Trim();
            category.IsActive = request.IsActive;

            category.UpdatedAt = DateTime.UtcNow;
            category.UpdatedBy = _currentUser.UserId;

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                "Product category updated successfully.");
        }
    }
}