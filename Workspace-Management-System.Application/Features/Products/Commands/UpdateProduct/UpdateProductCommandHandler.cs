using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler
    : IRequestHandler<UpdateProductCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly IStringLocalizer _localizer;

    public UpdateProductCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser,
        IStringLocalizerFactory factory)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _localizer = factory.Create(typeof(SharedResources));
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
                _localizer["ProductNotFound"]);
        }

        var categoryExists = await _unitOfWork.ProductCategories
            .Query()
            .AnyAsync(
                x =>
                    x.Id == request.ProductCategoryId &&
                    !x.IsDeleted,
                cancellationToken);

        if (!categoryExists)
        {
            return Result<bool>.Failure(
                ResultStatus.NotFound,
                _localizer["ProductCategoryNotFound"]);
        }

        var sku = request.Sku.Trim();

        var skuExists = await _unitOfWork.Products
            .Query()
            .AnyAsync(
                x =>
                    x.Id != request.Id &&
                    x.Sku.ToLower() == sku.ToLower() &&
                    !x.IsDeleted,
                cancellationToken);

        if (skuExists)
        {
            return Result<bool>.Failure(
                ResultStatus.Conflict,
                _localizer["ProductWithSameSkuAlreadyExists"]);
        }

        var nameEn = request.NameEn.Trim();
        var nameAr = request.NameAr.Trim();

        var nameExists = await _unitOfWork.Products
            .Query()
            .AnyAsync(
                x =>
                    x.Id != request.Id &&
                    !x.IsDeleted &&
                    (
                        x.NameEn.ToLower() == nameEn.ToLower() ||
                        x.NameAr.ToLower() == nameAr.ToLower()
                    ),
                cancellationToken);

        if (nameExists)
        {
            return Result<bool>.Failure(
                ResultStatus.Conflict,
                _localizer["ProductWithSameNameAlreadyExists"]);
        }

        product.ProductCategoryId = request.ProductCategoryId;
        product.NameEn = nameEn;
        product.NameAr = nameAr;
        product.DescriptionEn = request.DescriptionEn?.Trim();
        product.DescriptionAr = request.DescriptionAr?.Trim();
        product.Sku = sku;
        product.SellingPrice = request.SellingPrice;
        product.CostPrice = request.CostPrice;
        product.IsActive = request.IsActive;
        product.UpdatedAt = DateTime.UtcNow;
        product.UpdatedBy = _currentUser.UserId;

        await _unitOfWork.SaveAsync();

        return Result<bool>.Success(
            true,
            _localizer["ProductUpdatedSuccessfully"]);
    }
}