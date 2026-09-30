using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.PaginatedResults;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Packages.Dtos;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Packages.Queries.GetPackages;

public class GetPackagesHandler
    : IRequestHandler<
        GetPackagesQuery,
        Result<PaginatedResult<PackageDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPackagesHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PaginatedResult<PackageDto>>> Handle(
        GetPackagesQuery request,
        CancellationToken cancellationToken)
    {
        var query = _unitOfWork.Packages
            .Query()
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();

            query = query.Where(x =>
                x.Name.Contains(search) ||
                (x.Description != null &&
                 x.Description.Contains(search)));
        }

        var totalCount = await query
            .CountAsync(cancellationToken);

        var packages = await query
            .OrderBy(x => x.Name)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
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
            .ToListAsync(cancellationToken);

        var result = new PaginatedResult<PackageDto>(
            packages,
            request.PageNumber,
            request.PageSize,
            totalCount);

        return Result<PaginatedResult<PackageDto>>.Success(
            result,
            "Packages retrieved successfully.");
    }
}