using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Globalization;
using Workspace_Management_System.Application.Common.PaginatedResults;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Products.Queries.GetProducts;

public class GetProductsQueryHandler
    : IRequestHandler<
        GetProductsQuery,
        Result<PaginatedResult<ProductDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public GetProductsQueryHandler(
        IUnitOfWork unitOfWork,
        IStringLocalizer<SharedResources> localizer)
    {
        _unitOfWork = unitOfWork;
        _localizer = localizer;
    }

    public async Task<Result<PaginatedResult<ProductDto>>> Handle(
        GetProductsQuery request,
        CancellationToken cancellationToken)
    {
        var isArabic =
            CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "ar";

        var query = _unitOfWork.Products
            .Query()
            .AsNoTracking()
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();

            query = query.Where(x =>
                x.NameEn.Contains(search) ||
                x.NameAr.Contains(search) ||
                (x.DescriptionEn != null &&
                 x.DescriptionEn.Contains(search)) ||
                (x.DescriptionAr != null &&
                 x.DescriptionAr.Contains(search)) ||
                x.Sku.Contains(search));
        }

        if (request.ProductCategoryId.HasValue)
        {
            query = query.Where(x =>
                x.ProductCategoryId ==
                request.ProductCategoryId.Value);
        }

        if (request.IsActive.HasValue)
        {
            query = query.Where(x =>
                x.IsActive == request.IsActive.Value);
        }

        var totalCount = await query.CountAsync(
            cancellationToken);

        var items = await query
            .OrderBy(x => isArabic
                ? x.NameAr
                : x.NameEn)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new ProductDto
            {
                Id = x.Id,
                ProductCategoryId = x.ProductCategoryId,
                ProductCategoryName = isArabic
                    ? x.ProductCategory.NameAr
                    : x.ProductCategory.NameEn,
                Name = isArabic
                    ? x.NameAr
                    : x.NameEn,
                Description = isArabic
                    ? x.DescriptionAr
                    : x.DescriptionEn,
                Sku = x.Sku,
                SellingPrice = x.SellingPrice,
                CostPrice = x.CostPrice,
                IsActive = x.IsActive,
                IsDeleted = x.IsDeleted
            })
            .ToListAsync(cancellationToken);

        var result = new PaginatedResult<ProductDto>(
            items,
            request.PageNumber,
            request.PageSize,
            totalCount);

        return Result<PaginatedResult<ProductDto>>.Success(
            result,
            _localizer["ProductsRetrievedSuccessfully"]);
    }
}
