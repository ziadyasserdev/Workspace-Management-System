using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Globalization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.ProductCategories.DTOs;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.ProductCategories.Queries.GetProductCategoryById;

public class GetProductCategoryByIdQueryHandler
    : IRequestHandler<
        GetProductCategoryByIdQuery,
        Result<ProductCategoryDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public GetProductCategoryByIdQueryHandler(
        IUnitOfWork unitOfWork,
        IStringLocalizer<SharedResources> localizer)
    {
        _unitOfWork = unitOfWork;
        _localizer = localizer;
    }

    public async Task<Result<ProductCategoryDto>> Handle(
        GetProductCategoryByIdQuery request,
        CancellationToken cancellationToken)
    {
        var isArabic =
            CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "ar";

        var category = await _unitOfWork.ProductCategories
            .Query()
            .AsNoTracking()
            .Where(x =>
                x.Id == request.Id &&
                !x.IsDeleted)
            .Select(x => new ProductCategoryDto
            {
                Id = x.Id,
                Name = isArabic
                    ? x.NameAr
                    : x.NameEn,
                Description = isArabic
                    ? x.DescriptionAr
                    : x.DescriptionEn,
                IsActive = x.IsActive,
                IsDeleted = x.IsDeleted
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (category == null)
        {
            return Result<ProductCategoryDto>.Failure(
                ResultStatus.NotFound,
                _localizer["ProductCategoryNotFound"]);
        }

        return Result<ProductCategoryDto>.Success(
            category,
            _localizer["ProductCategoryRetrievedSuccessfully"]);
    }
}
