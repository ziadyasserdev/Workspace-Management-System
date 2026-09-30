using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Packages.Dtos;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Packages.Queries.GetPackageById;

public class GetPackageByIdHandler
    : IRequestHandler<GetPackageByIdQuery, Result<PackageDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPackageByIdHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PackageDto>> Handle(
        GetPackageByIdQuery request,
        CancellationToken cancellationToken)
    {
        var package = await _unitOfWork.Packages
            .Query()
            .Where(x =>
                x.Id == request.Id &&
                !x.IsDeleted)
            .Select(x => new PackageDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                PackageType = x.PackageType,
                TotalHours = x.TotalHours,
                DurationDays = x.DurationDays,
                Price = x.Price,
                IsActive = x.IsActive,

                CustomerCount = x.CustomerPackages
                    .Count(cp =>
                        !cp.IsDeleted &&
                        !cp.Customer.IsDeleted &&
                        cp.Status == CustomerPackageStatus.Active),

                Customers = x.CustomerPackages
                    .Where(cp =>
                        !cp.IsDeleted &&
                        !cp.Customer.IsDeleted &&
                        cp.Status == CustomerPackageStatus.Active)
                    .Select(cp => new PackageCustomerDto
                    {
                        CustomerId = cp.CustomerId,
                        FullName = cp.Customer.FullName,
                        MobileNumber = cp.Customer.MobileNumber,
                        Status = cp.Status,
                        PurchaseDate = cp.PurchaseDate,
                        StartDate = cp.StartDate,
                        EndDate = cp.EndDate,
                        RemainingHours = cp.RemainingHours
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (package is null)
        {
            return Result<PackageDto>.Failure(
                ResultStatus.NotFound,
                "Package not found.");
        }

        return Result<PackageDto>.Success(
            package,
            "Package retrieved successfully.");
    }
}