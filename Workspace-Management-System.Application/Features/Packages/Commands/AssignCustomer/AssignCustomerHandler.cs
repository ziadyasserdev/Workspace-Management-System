using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Domain.Enums;
using Workspace_Management_System.Domain.Models;

namespace Workspace_Management_System.Application.Features.Packages.Commands.AssignCustomer;

public class AssignCustomerHandler
    : IRequestHandler<AssignCustomerCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    public AssignCustomerHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;

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
                "Customer not found.");
        }

        var package = await _unitOfWork.Packages
            .GetByIdAsync(request.PackageId);

        if (package is null)
        {
            return Result<int>.Failure(
                ResultStatus.NotFound,
                "Package not found.");
        }

        if (!package.IsActive)
        {
            return Result<int>.Failure(
                ResultStatus.Conflict,
                "Cannot assign an inactive package.");
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
                "Customer already has an active package.");
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
            "Customer assigned to package successfully.");
    }
}