using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Products.Commands.RestoreProduct;

public class RestoreProductCommandHandler
    : IRequestHandler<RestoreProductCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly IStringLocalizer _localizer;

    public RestoreProductCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser,
        IStringLocalizerFactory factory)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _localizer = factory.Create(typeof(SharedResources));
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
                _localizer["DeletedProductNotFound"]);
        }

        var skuExists = await _unitOfWork.Products
            .Query()
            .AnyAsync(
                x =>
                    x.Id != request.Id &&
                    x.Sku.ToLower() == product.Sku.ToLower() &&
                    !x.IsDeleted,
                cancellationToken);

        if (skuExists)
        {
            return Result<bool>.Failure(
                ResultStatus.Conflict,
                _localizer["ProductWithSameSkuAlreadyExists"]);
        }

        var nameExists = await _unitOfWork.Products
            .Query()
            .AnyAsync(
                x =>
                    x.Id != request.Id &&
                    !x.IsDeleted &&
                    (
                        x.NameEn.ToLower() == product.NameEn.ToLower() ||
                        x.NameAr.ToLower() == product.NameAr.ToLower()
                    ),
                cancellationToken);

        if (nameExists)
        {
            return Result<bool>.Failure(
                ResultStatus.Conflict,
                _localizer["ProductWithSameNameAlreadyExists"]);
        }

        var categoryExists = await _unitOfWork.ProductCategories
            .Query()
            .AnyAsync(
                x =>
                    x.Id == product.ProductCategoryId &&
                    !x.IsDeleted,
                cancellationToken);

        if (!categoryExists)
        {
            return Result<bool>.Failure(
                ResultStatus.Conflict,
                _localizer["ProductCategoryDeletedOrNotFound"]);
        }

        product.IsDeleted = false;
        product.IsActive = true;
        product.UpdatedAt = DateTime.UtcNow;
        product.UpdatedBy = _currentUser.UserId;
        product.IsDeletedBy = null;

        await _unitOfWork.SaveAsync();

        return Result<bool>.Success(
            true,
            _localizer["ProductRestoredSuccessfully"]);
    }
}