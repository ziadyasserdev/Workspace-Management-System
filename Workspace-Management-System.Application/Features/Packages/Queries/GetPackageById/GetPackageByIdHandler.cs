using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.ComponentModel;
using System.Globalization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Packages.Dtos;
using Workspace_Management_System.Application.Resources;
using Workspace_Management_System.Domain.Enums;
using Workspace_Management_System.Domain.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.Mime.MediaTypeNames;

namespace Workspace_Management_System.Application.Features.Packages.Queries.GetPackageById;

public class GetPackageByIdHandler
    : IRequestHandler<GetPackageByIdQuery, Result<PackageDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public GetPackageByIdHandler(
        IUnitOfWork unitOfWork,
        IStringLocalizer<SharedResources> localizer)
    {
        _unitOfWork = unitOfWork;
        _localizer = localizer;
    }

    public async Task<Result<PackageDto>> Handle(
        GetPackageByIdQuery request,
        CancellationToken cancellationToken)
    {
        var isArabic =
            CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "ar";

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
                _localizer["PackageNotFound"]);
        }

        var result = new PackageDto
        {
            Id = package.Id,
            Name = isArabic
                ? package.NameAr
                : package.NameEn,
            Description = isArabic
                ? package.DescriptionAr
                : package.DescriptionEn,
            PackageType = _localizer[
                $"PackageType_{package.PackageType}"],
            TotalHours = package.TotalHours,
            DurationDays = package.DurationDays,
            Price = package.Price,
            IsActive = package.IsActive,
            CustomerCount = package.CustomerCount,
            Customers = package.Customers
                .Select(cp => new PackageCustomerDto
                {
                    CustomerId = cp.CustomerId,
                    FullName = isArabic
                        ? cp.CustomerNameAr
                        : cp.CustomerNameEn,
                    MobileNumber = cp.MobileNumber,
                    Status = _localizer[
                        $"CustomerPackageStatus_{cp.Status}"],
                    PurchaseDate = cp.PurchaseDate,
                    StartDate = cp.StartDate,
                    EndDate = cp.EndDate,
                    RemainingHours = cp.RemainingHours
                })
                .ToList()
        };

        return Result<PackageDto>.Success(
            result,
            _localizer["PackageRetrievedSuccessfully"]);
    }
}
