
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Resources;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Packages.Commands.DeactivatePackage;

public class DeactivatePackageHandler
    : IRequestHandler<DeactivatePackageCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly IStringLocalizer _localizer;

    public DeactivatePackageHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser,
        IStringLocalizerFactory factory)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _localizer = factory.Create(typeof(SharedResources));
    }

    public async Task<Result<int>> Handle(
        DeactivatePackageCommand request,
        CancellationToken cancellationToken)
    {
        var package = await _unitOfWork.Packages
            .GetByIdAsync(request.Id);

        if (package is null)
        {
            return Result<int>.Failure(
                ResultStatus.NotFound,
                _localizer["PackageNotFound"]);
        }

        if (!package.IsActive)
        {
            return Result<int>.Failure(
                ResultStatus.Conflict,
                _localizer["PackageAlreadyInactive"]);
        }

        var hasActiveCustomers = await _unitOfWork.CustomerPackages
            .Query()
            .AnyAsync(
                x =>
                    x.PackageId == request.Id &&
                    x.Status == CustomerPackageStatus.Active &&
                    !x.IsDeleted,
                cancellationToken);

        if (hasActiveCustomers)
        {
            return Result<int>.Failure(
                ResultStatus.Conflict,
                _localizer["CannotDeactivatePackageWithActiveCustomers"]);
        }

        package.IsActive = false;
        package.UpdatedAt = DateTime.UtcNow;
        package.UpdatedBy = _currentUser.UserId;

        _unitOfWork.Packages.Update(package);
        await _unitOfWork.SaveAsync();

        return Result<int>.Success(
            package.Id,
            _localizer["PackageDeactivatedSuccessfully"]);
    }
}

