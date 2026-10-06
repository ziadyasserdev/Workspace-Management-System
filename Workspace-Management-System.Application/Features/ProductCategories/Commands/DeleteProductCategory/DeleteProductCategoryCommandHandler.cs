using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.ProductCategories.Commands.DeleteProductCategory;

public class DeleteProductCategoryCommandHandler
    : IRequestHandler<DeleteProductCategoryCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly IStringLocalizer _localizer;

    public DeleteProductCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser,
        IStringLocalizerFactory factory)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _localizer = factory.Create(typeof(SharedResources));
    }

    public async Task<Result<bool>> Handle(
        DeleteProductCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var category = await _unitOfWork.ProductCategories
            .Query()
            .FirstOrDefaultAsync(
                x => x.Id == request.Id && !x.IsDeleted,
                cancellationToken);

        if (category == null)
        {
            return Result<bool>.Failure(
                ResultStatus.NotFound,
                _localizer["ProductCategoryNotFound"]);
        }

        category.IsDeleted = true;
        category.IsActive = false;
        category.IsDeletedBy = _currentUser.UserId;
        category.UpdatedAt = DateTime.UtcNow;
        category.UpdatedBy = _currentUser.UserId;

        await _unitOfWork.SaveAsync();

        return Result<bool>.Success(
            true,
            _localizer["ProductCategoryDeletedSuccessfully"]);
    }
}