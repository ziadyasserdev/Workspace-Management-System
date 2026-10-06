using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Resources;
using Workspace_Management_System.Domain.Enums;
using Workspace_Management_System.Domain.Models;

namespace Workspace_Management_System.Application.Features.Packages.Commands.AssignCustomer;

public class AssignCustomerHandler
    : IRequestHandler<AssignCustomerCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly IStringLocalizer _localizer;

    public AssignCustomerHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser,
        IStringLocalizerFactory factory)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _localizer = factory.Create(typeof(SharedResources));
    }

    public async Task<Result<int>> Handle(
        AssignCustomerCommand request,
        CancellationToken cancellationToken)
    {
        var customer = await _unitOfWork.Customers
            .Query()
            .FirstOrDefaultAsync(
                x =>
                    x.Id == request.CustomerId &&
                    !x.IsDeleted,
                cancellationToken);

        if (customer is null)
        {
            return Result<int>.Failure(
                ResultStatus.NotFound,
                _localizer["CustomerNotFound"]);
        }

        var package = await _unitOfWork.Packages
            .GetByIdAsync(request.PackageId);

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
                _localizer["CannotAssignInactivePackage"]);
        }

        var existingActivePackage =
            await _unitOfWork.CustomerPackages
                .Query()
                .FirstOrDefaultAsync(
                    x =>
                        x.CustomerId == request.CustomerId &&
                        x.Status == CustomerPackageStatus.Active &&
                        !x.IsDeleted,
                    cancellationToken);

        if (existingActivePackage is not null)
        {
            return Result<int>.Failure(
                ResultStatus.Conflict,
                _localizer["CustomerAlreadyHasActivePackage"]);
        }

        var now = DateTime.UtcNow;

        DateTime? endDate = package.DurationDays.HasValue
            ? now.AddDays(package.DurationDays.Value)
            : null;

        var customerPackage = new CustomerPackage
        {
            CustomerId = request.CustomerId,
            PackageId = request.PackageId,
            PurchaseDate = now,
            StartDate = now,
            EndDate = endDate,
            RemainingHours = package.TotalHours,
            Status = CustomerPackageStatus.Active,
            CreatedAt = now,
            CreatedBy = _currentUser.UserId
        };

        await _unitOfWork.CustomerPackages
            .AddAsync(customerPackage);

        await _unitOfWork.SaveAsync();

        return Result<int>.Success(
            customerPackage.Id,
            _localizer["CustomerAssignedToPackageSuccessfully"]);
    }
}

