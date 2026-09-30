using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Domain.Enums;
using Workspace_Management_System.Domain.Models;

namespace Workspace_Management_System.Application.Features.Packages.Commands.UpgradeCustomerPackage;

public class UpgradeCustomerPackageHandler
    : IRequestHandler<UpgradeCustomerPackageCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpgradeCustomerPackageHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(
        UpgradeCustomerPackageCommand request,
        CancellationToken cancellationToken)
    {
        await using var transaction =
            await _unitOfWork.BeginTransactionAsync();

        try
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

            var currentCustomerPackage =
                await _unitOfWork.CustomerPackages
                    .Query()
                    .FirstOrDefaultAsync(
                        x =>
                            x.CustomerId == request.CustomerId &&
                            x.Status == CustomerPackageStatus.Active &&
                            !x.IsDeleted,
                        cancellationToken);

            if (currentCustomerPackage is null)
            {
                return Result<int>.Failure(
                    ResultStatus.NotFound,
                    "Customer does not have an active package.");
            }

            var newPackage = await _unitOfWork.Packages
                .GetByIdAsync(request.NewPackageId);

            if (newPackage is null)
            {
                return Result<int>.Failure(
                    ResultStatus.NotFound,
                    "New package not found.");
            }

            if (!newPackage.IsActive)
            {
                return Result<int>.Failure(
                    ResultStatus.Conflict,
                    "Cannot upgrade to an inactive package.");
            }

            if (currentCustomerPackage.PackageId ==
                request.NewPackageId)
            {
                return Result<int>.Failure(
                    ResultStatus.Conflict,
                    "Customer already has this package.");
            }

            var now = DateTime.UtcNow;

            currentCustomerPackage.Status =
                CustomerPackageStatus.Upgraded;

            currentCustomerPackage.EndDate = now;
            currentCustomerPackage.UpdatedAt = now;

            _unitOfWork.CustomerPackages
                .Update(currentCustomerPackage);

            DateTime? endDate = newPackage.DurationDays.HasValue
                ? now.AddDays(newPackage.DurationDays.Value)
                : null;

            var newCustomerPackage = new CustomerPackage
            {
                CustomerId = request.CustomerId,
                PackageId = request.NewPackageId,

                PurchaseDate = now,
                StartDate = now,
                EndDate = endDate,

                RemainingHours = newPackage.TotalHours,

                Status = CustomerPackageStatus.Active,

                CreatedAt = now
            };

            await _unitOfWork.CustomerPackages
                .AddAsync(newCustomerPackage);

            await _unitOfWork.SaveAsync();

            await transaction.CommitAsync(cancellationToken);

            return Result<int>.Success(
                newCustomerPackage.Id,
                "Customer package upgraded successfully.");
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}