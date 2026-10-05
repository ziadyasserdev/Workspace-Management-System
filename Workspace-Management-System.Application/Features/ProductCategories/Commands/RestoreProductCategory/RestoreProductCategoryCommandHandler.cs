using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;

namespace Workspace_Management_System.Application.Features.ProductCategories.Commands.RestoreProductCategory
{
    public class RestoreProductCategoryCommandHandler
        : IRequestHandler<RestoreProductCategoryCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public RestoreProductCategoryCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<bool>> Handle(
            RestoreProductCategoryCommand request,
            CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.ProductCategories
                .Query()
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id,
                    cancellationToken);

            if (category == null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    "Product category not found.");
            }

            if (!category.IsDeleted)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "Product category is not deleted.");
            }

            var duplicate = await _unitOfWork.ProductCategories
                .Query()
                .IgnoreQueryFilters()
                .AnyAsync(
                    x =>
                        x.Id != request.Id &&
                        !x.IsDeleted &&
                        (
                            x.NameEn.ToLower() == category.NameEn.ToLower() ||
                            x.NameAr.ToLower() == category.NameAr.ToLower()
                        ),
                    cancellationToken);

            if (duplicate)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "Cannot restore category because another active category has the same name.");
            }

            category.IsDeleted = false;
            category.IsActive = false;
            category.IsDeletedBy = null;
            category.UpdatedAt = DateTime.UtcNow;
            category.UpdatedBy = _currentUser.UserId;

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                "Product category restored successfully.");
        }
    }
}