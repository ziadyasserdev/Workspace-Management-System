using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Packages.Dtos;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Packages.Queries.GetPackageById;

public class GetPackageByIdHandler
    : IRequestHandler<GetPackageByIdQuery, Result<PackageDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;

    public GetPackageByIdHandler(
        IUnitOfWork unitOfWork,
        ILocalizationService localizationService)
    {
        _unitOfWork = unitOfWork;
        _localizationService = localizationService;
    }

    public async Task<Result<PackageDto>> Handle(
        GetPackageByIdQuery request,
        CancellationToken cancellationToken)
    {
        var package = await _unitOfWork.Packages
            .Query()
            .AsNoTracking()
            .Where(x =>
                x.Id == request.Id &&
                !x.IsDeleted)
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
            .FirstOrDefaultAsync(cancellationToken);

        if (package is null)
        {
            return Result<PackageDto>.Failure(
                ResultStatus.NotFound,
                "Package not found.");
        }

        var result = new PackageDto
        {
            Id = package.Id,
            NameEn = package.NameEn,
            NameAr = package.NameAr,
            DescriptionEn = package.DescriptionEn,
            DescriptionAr = package.DescriptionAr,
            PackageType = package.PackageType,
            TotalHours = package.TotalHours,
            DurationDays = package.DurationDays,
            Price = package.Price,
            IsActive = package.IsActive,
            CustomerCount = package.CustomerCount,
            Customers = package.Customers
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
        };

        return Result<PackageDto>.Success(
            result,
            "Package retrieved successfully.");
    }
}