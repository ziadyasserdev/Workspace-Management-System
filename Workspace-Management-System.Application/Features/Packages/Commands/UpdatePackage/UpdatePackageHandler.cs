using MediatR;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Resources;
using Workspace_Management_System.Domain.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.Mime.MediaTypeNames;

namespace Workspace_Management_System.Application.Features.Packages.Commands.UpdatePackage;

public class UpdatePackageHandler
    : IRequestHandler<UpdatePackageCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly IStringLocalizer _localizer;

    public UpdatePackageHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser,
        IStringLocalizerFactory factory)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _localizer = factory.Create(typeof(SharedResources));
    }

    public async Task<Result<int>> Handle(
        UpdatePackageCommand request,
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

        package.NameEn = request.NameEn.Trim();
        package.NameAr = request.NameAr.Trim();

        package.DescriptionEn =
            string.IsNullOrWhiteSpace(request.DescriptionEn)
                ? null
                : request.DescriptionEn.Trim();

        package.DescriptionAr =
            string.IsNullOrWhiteSpace(request.DescriptionAr)
                ? null
                : request.DescriptionAr.Trim();

        package.PackageType = request.PackageType;
        package.TotalHours = request.TotalHours;
        package.DurationDays = request.DurationDays;
        package.Price = request.Price;
        package.IsActive = request.IsActive;

        package.UpdatedAt = DateTime.UtcNow;
        package.UpdatedBy = _currentUser.UserId;

        _unitOfWork.Packages.Update(package);

        await _unitOfWork.SaveAsync();

        return Result<int>.Success(
            package.Id,
            _localizer["PackageUpdatedSuccessfully"]);
    }
}
