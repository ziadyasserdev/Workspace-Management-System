
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.ProductCategories.DTOs;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.ProductCategories.Queries.GetProductCategoryById;

public class GetProductCategoryByIdQueryHandler
    : IRequestHandler<GetProductCategoryByIdQuery, Result<ProductCategoryEditDto>>
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

    public async Task<Result<ProductCategoryEditDto>> Handle(
        GetProductCategoryByIdQuery request,
        CancellationToken cancellationToken)
    {
        var category = await _unitOfWork.ProductCategories
            .Query()
            .AsNoTracking()
            .Where(x => x.Id == request.Id && !x.IsDeleted)
            .Select(x => new ProductCategoryEditDto
            {
                Id = x.Id,
                NameEn = x.NameEn,
                NameAr = x.NameAr,
                DescriptionEn = x.DescriptionEn,
                DescriptionAr = x.DescriptionAr,
                IsActive = x.IsActive,
                IsDeleted = x.IsDeleted
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (category is null)
        {
            return Result<ProductCategoryEditDto>.Failure(
                ResultStatus.NotFound,
                _localizer["ProductCategoryNotFound"]);
        }

        return Result<ProductCategoryEditDto>.Success(
            category,
            _localizer["ProductCategoryRetrievedSuccessfully"]);
    }
}
