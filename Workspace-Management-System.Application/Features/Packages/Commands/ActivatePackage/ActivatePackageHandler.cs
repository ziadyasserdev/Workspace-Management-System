using MediatR;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;

namespace Workspace_Management_System.Application.Features.Packages.Commands.ActivatePackage;

public class ActivatePackageHandler
    : IRequestHandler<ActivatePackageCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public ActivatePackageHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
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
                "Package not found.");
        }

        if (package.IsActive)
        {
            return Result<int>.Failure(
                ResultStatus.Conflict,
                "Package is already active.");
        }

        package.IsActive = true;
        package.UpdatedAt = DateTime.UtcNow;
        package.UpdatedBy = _currentUser.UserId;

        _unitOfWork.Packages.Update(package);
        await _unitOfWork.SaveAsync();

        return Result<int>.Success(
            package.Id,
            "Package activated successfully.");
    }
}