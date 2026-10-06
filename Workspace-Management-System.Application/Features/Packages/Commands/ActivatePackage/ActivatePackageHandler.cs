
using MediatR;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Packages.Commands.ActivatePackage;

public class ActivatePackageHandler
    : IRequestHandler<ActivatePackageCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly IStringLocalizer _localizer;

    public ActivatePackageHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser,
        IStringLocalizerFactory factory)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _localizer = factory.Create(typeof(SharedResources));
    }

    public async Task<Result<int>> Handle(
        ActivatePackageCommand request,
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

        if (package.IsActive)
        {
            return Result<int>.Failure(
                ResultStatus.Conflict,
                _localizer["PackageAlreadyActive"]);
        }

        package.IsActive = true;
        package.UpdatedAt = DateTime.UtcNow;
        package.UpdatedBy = _currentUser.UserId;

        _unitOfWork.Packages.Update(package);
        await _unitOfWork.SaveAsync();

        return Result<int>.Success(
            package.Id,
            _localizer["PackageActivatedSuccessfully"]);
    }
}

