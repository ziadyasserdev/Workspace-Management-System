using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Packages.Commands.DeletePackage;

public class DeletePackageHandler
    : IRequestHandler<DeletePackageCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public DeletePackageHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result<int>> Handle(
        DeletePackageCommand request,
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
                "Cannot delete a package with active customers.");
        }

        package.IsDeleted = true;
        package.IsActive = false;
        package.IsDeletedBy = _currentUser.UserId;
        package.UpdatedAt = DateTime.UtcNow;
        package.UpdatedBy = _currentUser.UserId;

        _unitOfWork.Packages.Update(package);

        await _unitOfWork.SaveAsync();

        return Result<int>.Success(
            package.Id,
            "Package deleted successfully.");
    }
}