
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Products.DTOs;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Products.Queries.GetProductById;

public class GetProductByIdQueryHandler
    : IRequestHandler<GetProductByIdQuery, Result<ProductEditDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public GetProductByIdQueryHandler(
        IUnitOfWork unitOfWork,
        IStringLocalizer<SharedResources> localizer)
    {
        _unitOfWork = unitOfWork;
        _localizer = localizer;
    }

    public async Task<Result<ProductEditDto>> Handle(
        GetProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.Products
            .Query()
            .AsNoTracking()
            .Where(x => x.Id == request.Id && !x.IsDeleted)
            .Select(x => new ProductEditDto
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

        if (product is null)
        {
            return Result<ProductEditDto>.Failure(
                ResultStatus.NotFound,
                _localizer["ProductNotFound"]);
        }

        return Result<ProductEditDto>.Success(
            product,
            _localizer["ProductRetrievedSuccessfully"]);
    }
}
