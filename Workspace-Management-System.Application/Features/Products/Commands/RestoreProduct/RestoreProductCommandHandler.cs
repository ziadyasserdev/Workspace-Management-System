using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;

namespace Workspace_Management_System.Application.Features.Products.Commands.RestoreProduct
{
    public class RestoreProductCommandHandler
        : IRequestHandler<RestoreProductCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public RestoreProductCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<bool>> Handle(
            RestoreProductCommand request,
            CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Products
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && x.IsDeleted,
                    cancellationToken);

            if (product == null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    "Deleted product not found.");
            }

            var skuExists = await _unitOfWork.Products
                .Query()
                .AnyAsync(
                    x => x.Id != request.Id
                         && x.Sku.ToLower() == product.Sku.ToLower()
                         && !x.IsDeleted,
                    cancellationToken);

            if (skuExists)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "A product with the same SKU already exists.");
            }

            var nameExists = await _unitOfWork.Products
                .Query()
                .AnyAsync(
                    x => x.Id != request.Id
                         && x.EnglishName.ToLower() == product.EnglishName.ToLower()
                         && !x.IsDeleted,
                    cancellationToken);

            if (nameExists)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "A product with the same English name already exists.");
            }

            var categoryExists = await _unitOfWork.ProductCategories
                .Query()
                .AnyAsync(
                    x => x.Id == product.ProductCategoryId
                         && !x.IsDeleted,
                    cancellationToken);

            if (!categoryExists)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "The product category is deleted or no longer exists.");
            }

            product.IsDeleted = false;
            product.IsActive = true;
            product.UpdatedAt = DateTime.UtcNow;
            product.UpdatedBy = _currentUser.UserId;
            product.IsDeletedBy = null;
            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                "Product restored successfully.");
        }
    }
}