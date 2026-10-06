using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Resources;
using Workspace_Management_System.Domain.Models;

namespace Workspace_Management_System.Application.Features.ProductCategories.Commands.CreateProductCategory;

public class CreateProductCategoryCommandHandler
    : IRequestHandler<CreateProductCategoryCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly IStringLocalizer _localizer;

    public CreateProductCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser,
        IStringLocalizerFactory factory)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _localizer = factory.Create(typeof(SharedResources));
    }

    public async Task<Result<int>> Handle(
        CreateProductCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var nameEn = request.NameEn.Trim();
        var nameAr = request.NameAr.Trim();

        var nameExists = await _unitOfWork.ProductCategories
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
                _localizer["ProductCategoryWithSameNameAlreadyExists"]);
        }

        var category = new ProductCategory
        {
            NameEn = nameEn,
            NameAr = nameAr,
            DescriptionEn = request.DescriptionEn?.Trim(),
            DescriptionAr = request.DescriptionAr?.Trim(),
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = _currentUser.UserId
        };

        await _unitOfWork.ProductCategories.AddAsync(category);

        await _unitOfWork.SaveAsync();

        return Result<int>.Success(
            category.Id,
            _localizer["ProductCategoryCreatedSuccessfully"]);
    }
}