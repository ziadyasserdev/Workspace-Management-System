using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Packages.Commands.RestorePackage;

public class RestorePackageHandler
    : IRequestHandler<RestorePackageCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly IStringLocalizer _localizer;

    public RestorePackageHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser,
        IStringLocalizerFactory factory)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _localizer = factory.Create(typeof(SharedResources));
    }

    public async Task<Result<int>> Handle(
        RestorePackageCommand request,
        CancellationToken cancellationToken)
    {
        var deletedPackage = await _unitOfWork.Packages
            .Query()
            .FirstOrDefaultAsync(
                x =>
                    x.Id == request.Id &&
                    x.IsDeleted,
                cancellationToken);

        if (deletedPackage is null)
        {
            var activePackage = await _unitOfWork.Packages
                .GetByIdAsync(request.Id);

            if (activePackage is not null)
            {
                return Result<int>.Failure(
                    ResultStatus.Conflict,
                    _localizer["PackageAlreadyActiveOrNotDeleted"]);
            }

            return Result<int>.Failure(
                ResultStatus.NotFound,
                _localizer["DeletedPackageNotFound"]);
        }

        deletedPackage.IsDeleted = false;
        deletedPackage.IsDeletedBy = null;
        deletedPackage.IsActive = true;
        deletedPackage.UpdatedAt = DateTime.UtcNow;
        deletedPackage.UpdatedBy = _currentUser.UserId;

        _unitOfWork.Packages.Update(deletedPackage);

        await _unitOfWork.SaveAsync();

        return Result<int>.Success(
            deletedPackage.Id,
            _localizer["PackageRestoredSuccessfully"]);
    }
}
