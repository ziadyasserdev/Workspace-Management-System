using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Domain.Models;

namespace Workspace_Management_System.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler
        : IRequestHandler<CreateProductCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public CreateProductCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<int>> Handle(
            CreateProductCommand request,
            CancellationToken cancellationToken)
        {
            var categoryExists = await _unitOfWork.ProductCategories
                .Query()
                .AnyAsync(
                    x => x.Id == request.ProductCategoryId
                         && !x.IsDeleted,
                    cancellationToken);

            if (!categoryExists)
            {
                return Result<int>.Failure(
                    ResultStatus.NotFound,
                    "Product category not found.");
            }

            var sku = request.Sku.Trim();

            var skuExists = await _unitOfWork.Products
                .Query()
                .AnyAsync(
                    x => x.Sku.ToLower() == sku.ToLower()
                         && !x.IsDeleted,
                    cancellationToken);

            if (skuExists)
            {
                return Result<int>.Failure(
                    ResultStatus.Conflict,
                    "A product with the same SKU already exists.");
            }

            var nameEn = request.NameEn.Trim();
            var nameAr = request.NameAr.Trim();

            var nameExists = await _unitOfWork.Products
                .Query()
                .AnyAsync(
                    x =>
                        !x.IsDeleted &&
                        (
                            x.NameEn.ToLower() == nameEn.ToLower() ||
                            x.NameAr.ToLower() == nameAr.ToLower()
                        ),
                    cancellationToken);

            if (nameExists)
            {
                return Result<int>.Failure(
                    ResultStatus.Conflict,
                    "A product with the same name already exists.");
            }

            var product = new Product
            {
                ProductCategoryId = request.ProductCategoryId,
                NameEn = nameEn,
                NameAr = nameAr,
                DescriptionEn = request.DescriptionEn?.Trim(),
                DescriptionAr = request.DescriptionAr?.Trim(),
                Sku = sku,
                SellingPrice = request.SellingPrice,
                CostPrice = request.CostPrice,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = _currentUser.UserId
            };

            await _unitOfWork.Products.AddAsync(product);

            await _unitOfWork.SaveAsync();

            return Result<int>.Success(
                product.Id,
                "Product created successfully.");
        }
    }
}