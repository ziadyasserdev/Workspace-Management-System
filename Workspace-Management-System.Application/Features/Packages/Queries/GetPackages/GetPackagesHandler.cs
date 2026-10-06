using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.PaginatedResults;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts;
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
    private readonly ILocalizationService _localizationService;

    public GetPackagesHandler(
        IUnitOfWork unitOfWork,
        ILocalizationService localizationService)
    {
        _unitOfWork = unitOfWork;
        _localizationService = localizationService;
    }

    public async Task<Result<PaginatedResult<PackageDto>>> Handle(
        GetPackagesQuery request,
        CancellationToken cancellationToken)
    {
        var query = _unitOfWork.Packages
            .Query()
            .AsNoTracking()
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();

            query = query.Where(x =>
                x.NameEn.Contains(search) ||
                x.NameAr.Contains(search) ||
                (x.DescriptionEn != null &&
                 x.DescriptionEn.Contains(search)) ||
                (x.DescriptionAr != null &&
                 x.DescriptionAr.Contains(search)));
        }

        var totalCount = await query
            .CountAsync(cancellationToken);

        var packages = await query
            .OrderBy(x => x.NameEn)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new
            {
                x.Id,
                x.NameEn,
                x.NameAr,
                x.DescriptionEn,
                x.DescriptionAr,
                x.PackageType,
                x.TotalHours,
                x.DurationDays,
                x.Price,
                x.IsActive,
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
                    .Select(cp => new
                    {
                        cp.CustomerId,
                        CustomerNameEn = cp.Customer.FullNameEn,
                        CustomerNameAr = cp.Customer.FullNameAr,
                        cp.Customer.MobileNumber,
                        cp.Status,
                        cp.PurchaseDate,
                        cp.StartDate,
                        cp.EndDate,
                        cp.RemainingHours
                    })
                    .ToList()
            })
            .ToListAsync(cancellationToken);

        var packageDtos = packages
            .Select(x => new PackageDto
            {
                Id = x.Id,
                NameEn = x.NameEn,
                NameAr = x.NameAr,
                DescriptionEn = x.DescriptionEn,
                DescriptionAr = x.DescriptionAr,
                PackageType = x.PackageType,
                TotalHours = x.TotalHours,
                DurationDays = x.DurationDays,
                Price = x.Price,
                IsActive = x.IsActive,
                CustomerCount = x.CustomerCount,
                Customers = x.Customers
                    .Select(cp => new PackageCustomerDto
                    {
                        CustomerId = cp.CustomerId,
                        FullName = _localizationService.GetLocalizedValue(
                            cp.CustomerNameEn,
                            cp.CustomerNameAr),
                        MobileNumber = cp.MobileNumber,
                        Status = cp.Status,
                        PurchaseDate = cp.PurchaseDate,
                        StartDate = cp.StartDate,
                        EndDate = cp.EndDate,
                        RemainingHours = cp.RemainingHours
                    })
                    .ToList()
            })
            .ToList();

        var result = new PaginatedResult<PackageDto>(
            packageDtos,
            request.PageNumber,
            request.PageSize,
            totalCount);

        return Result<PaginatedResult<PackageDto>>.Success(
            result,
            "Packages retrieved successfully.");
    }
}