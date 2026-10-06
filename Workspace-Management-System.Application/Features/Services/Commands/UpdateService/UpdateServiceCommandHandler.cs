using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Services.Commands.UpdateService;

public class UpdateServiceCommandHandler
    : IRequestHandler<UpdateServiceCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public UpdateServiceCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser,
        IStringLocalizer<SharedResources> localizer)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _localizer = localizer;
    }

    public async Task<Result<bool>> Handle(
        UpdateServiceCommand request,
        CancellationToken cancellationToken)
    {
        var service = await _unitOfWork.Services
            .Query()
            .FirstOrDefaultAsync(
                x => x.Id == request.Id && !x.IsDeleted,
                cancellationToken);

        if (service == null)
        {
            return Result<bool>.Failure(
                ResultStatus.NotFound,
                _localizer["ServiceNotFound"]);
        }

        var nameEn = request.NameEn.Trim();
        var nameAr = request.NameAr.Trim();

        var duplicateName = await _unitOfWork.Services
            .Query()
            .AnyAsync(
                x =>
                    x.Id != request.Id &&
                    !x.IsDeleted &&
                    (
                        x.NameEn == nameEn ||
                        x.NameAr == nameAr
                    ),
                cancellationToken);

        if (duplicateName)
        {
            return Result<bool>.Failure(
                ResultStatus.Conflict,
                _localizer["ServiceWithSameNameAlreadyExists"]);
        }

        service.NameEn = nameEn;
        service.NameAr = nameAr;
        service.DescriptionEn = request.DescriptionEn?.Trim();
        service.DescriptionAr = request.DescriptionAr?.Trim();
        service.Price = request.Price;
        service.IsActive = request.IsActive;
        service.UpdatedAt = DateTime.UtcNow;
        service.UpdatedBy = _currentUser.UserId;

        _unitOfWork.Services.Update(service);

        await _unitOfWork.SaveAsync();

        return Result<bool>.Success(
            true,
            _localizer["ServiceUpdatedSuccessfully"]);
    }
}