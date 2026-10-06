using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Services.Commands.RestoreService;

public class RestoreServiceCommandHandler
    : IRequestHandler<RestoreServiceCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public RestoreServiceCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser,
        IStringLocalizer<SharedResources> localizer)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _localizer = localizer;
    }

    public async Task<Result<bool>> Handle(
        RestoreServiceCommand request,
        CancellationToken cancellationToken)
    {
        var service = await _unitOfWork.Services
            .Query()
            .FirstOrDefaultAsync(
                x => x.Id == request.Id && x.IsDeleted,
                cancellationToken);

        if (service == null)
        {
            return Result<bool>.Failure(
                ResultStatus.NotFound,
                _localizer["DeletedServiceNotFound"]);
        }

        var duplicateName = await _unitOfWork.Services
            .Query()
            .AnyAsync(
                x =>
                    x.Id != request.Id &&
                    !x.IsDeleted &&
                    (
                        x.NameEn == service.NameEn ||
                        x.NameAr == service.NameAr
                    ),
                cancellationToken);

        if (duplicateName)
        {
            return Result<bool>.Failure(
                ResultStatus.Conflict,
                _localizer["ActiveServiceWithSameNameAlreadyExists"]);
        }

        var now = DateTime.UtcNow;

        service.IsDeleted = false;
        service.IsDeletedBy = null;
        service.IsActive = true;
        service.UpdatedAt = now;
        service.UpdatedBy = _currentUser.UserId;

        _unitOfWork.Services.Update(service);

        await _unitOfWork.SaveAsync();

        return Result<bool>.Success(
            true,
            _localizer["ServiceRestoredSuccessfully"]);
    }
}