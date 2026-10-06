using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Discounts.Commands.UpdateDiscount;

public class UpdateDiscountCommandHandler
    : IRequestHandler<UpdateDiscountCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly IStringLocalizer _localizer;

    public UpdateDiscountCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser,
        IStringLocalizerFactory factory)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _localizer = factory.Create(typeof(SharedResources));
    }

    public async Task<Result<bool>> Handle(
        UpdateDiscountCommand request,
        CancellationToken cancellationToken)
    {
        var discount = await _unitOfWork.Discounts
            .Query()
            .FirstOrDefaultAsync(
                x => x.Id == request.Id && !x.IsDeleted,
                cancellationToken);

        if (discount is null)
        {
            return Result<bool>.Failure(
                ResultStatus.NotFound,
                _localizer["DiscountNotFound"]);
        }

        var nameEn = request.NameEn.Trim();
        var nameAr = request.NameAr.Trim();

        var duplicate = await _unitOfWork.Discounts
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

        if (duplicate)
        {
            return Result<bool>.Failure(
                ResultStatus.Conflict,
                _localizer["DiscountSameNameAlreadyExists"]);
        }

        discount.NameEn = nameEn;
        discount.NameAr = nameAr;
        discount.DescriptionEn = request.DescriptionEn?.Trim();
        discount.DescriptionAr = request.DescriptionAr?.Trim();
        discount.DiscountType = request.Type;
        discount.Value = request.Value;
        discount.IsActive = request.IsActive;
        discount.StartDate = request.StartDate;
        discount.EndDate = request.EndDate;
        discount.UpdatedAt = DateTime.UtcNow;
        discount.UpdatedBy = _currentUser.UserId;

        _unitOfWork.Discounts.Update(discount);

        await _unitOfWork.SaveAsync();

        return Result<bool>.Success(
            true,
            _localizer["DiscountUpdatedSuccessfully"]);
    }
}
