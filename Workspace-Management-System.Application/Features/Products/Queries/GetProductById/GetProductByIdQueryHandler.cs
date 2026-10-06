using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Globalization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Products.Queries.GetProductById;

public class GetProductByIdQueryHandler
    : IRequestHandler<GetProductByIdQuery, Result<ProductDto>>
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

    public async Task<Result<ProductDto>> Handle(
        GetProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        var isArabic =
            CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "ar";

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
            .FirstOrDefaultAsync(cancellationToken);

        if (product == null)
        {
            return Result<ProductDto>.Failure(
                ResultStatus.NotFound,
                _localizer["ProductNotFound"]);
        }

        return Result<ProductDto>.Success(
            product,
            _localizer["ProductRetrievedSuccessfully"]);
    }
}
