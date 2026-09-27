using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;

namespace Workspace_Management_System.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler
        : IRequestHandler<UpdateProductCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public UpdateProductCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<bool>> Handle(
            UpdateProductCommand request,
            CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Products
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.IsDeleted,
                    cancellationToken);

            if (product == null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    "Product not found.");
            }

            var categoryExists = await _unitOfWork.ProductCategories
                .Query()
                .AnyAsync(
                    x => x.Id == request.ProductCategoryId
                         && !x.IsDeleted,
                    cancellationToken);

            if (!categoryExists)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    "Product category not found.");
            }

            var sku = request.Sku.Trim();

            var skuExists = await _unitOfWork.Products
                .Query()
                .AnyAsync(
                    x => x.Id != request.Id
                         && x.Sku.ToLower() == sku.ToLower()
                         && !x.IsDeleted,
                    cancellationToken);

            if (skuExists)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "A product with the same SKU already exists.");
            }

            var name = request.EnglishName.Trim();

            var nameExists = await _unitOfWork.Products
                .Query()
                .AnyAsync(
                    x => x.Id != request.Id
                         && x.EnglishName.ToLower() == name.ToLower()
                         && !x.IsDeleted,
                    cancellationToken);

            if (nameExists)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "A product with the same English name already exists.");
            }

            product.ProductCategoryId = request.ProductCategoryId;
            product.EnglishName = name;
            product.Description = request.Description?.Trim();
            product.Sku = sku;
            product.SellingPrice = request.SellingPrice;
            product.CostPrice = request.CostPrice;
            product.IsActive = request.IsActive;

            product.UpdatedAt = DateTime.UtcNow;
            product.UpdatedBy = _currentUser.UserId;

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(true,
                "Product updated successfully.");
        }
    }
}