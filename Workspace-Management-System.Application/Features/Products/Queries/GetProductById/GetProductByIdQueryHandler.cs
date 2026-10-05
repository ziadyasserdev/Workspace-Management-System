using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Products.DTOs;

namespace Workspace_Management_System.Application.Features.Products.Queries.GetProductById
{
    public class GetProductByIdQueryHandler
        : IRequestHandler<GetProductByIdQuery, Result<ProductDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetProductByIdQueryHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<ProductDto>> Handle(
            GetProductByIdQuery request,
            CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Products
                .Query()
                .AsNoTracking()
                .Where(x =>
                    x.Id == request.Id &&
                    !x.IsDeleted)
                .Select(x => new ProductDto
                {
                    Id = x.Id,
                    ProductCategoryId = x.ProductCategoryId,
                    ProductCategoryNameEn = x.ProductCategory.NameEn,
                    ProductCategoryNameAr = x.ProductCategory.NameAr,
                    NameEn = x.NameEn,
                    NameAr = x.NameAr,
                    DescriptionEn = x.DescriptionEn,
                    DescriptionAr = x.DescriptionAr,
                    Sku = x.Sku,
                    SellingPrice = x.SellingPrice,
                    CostPrice = x.CostPrice,
                    IsActive = x.IsActive,
                    IsDeleted = x.IsDeleted
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (product == null)
            {
                return Result<ProductDto>.Failure(
                    ResultStatus.NotFound,
                    "Product not found.");
            }

            return Result<ProductDto>.Success(
                product,
                "Product retrieved successfully.");
        }
    }
}