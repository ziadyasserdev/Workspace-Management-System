
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Packages.Dtos;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Packages.Queries.GetPackageById;

public class GetPackageByIdHandler
    : IRequestHandler<GetPackageByIdQuery, Result<PackageEditDto>>
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

    public async Task<Result<PackageEditDto>> Handle(
        GetPackageByIdQuery request,
        CancellationToken cancellationToken)
    {
        var package = await _unitOfWork.Packages
            .Query()
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == request.Id && !x.IsDeleted,
                cancellationToken);

        if (package is null)
        {
            return Result<PackageEditDto>.Failure(
                ResultStatus.NotFound,
                _localizer["PackageNotFound"]);
        }

        var result = new PackageEditDto
        {
            Id = package.Id,
            NameEn = package.NameEn,
            NameAr = package.NameAr,
            DescriptionEn = package.DescriptionEn,
            DescriptionAr = package.DescriptionAr,
            PackageType = package.PackageType.ToString(),
            TotalHours = package.TotalHours,
            DurationDays = package.DurationDays,
            Price = package.Price,
            IsActive = package.IsActive
        };

        return Result<PackageEditDto>.Success(
            result,
            _localizer["PackageRetrievedSuccessfully"]);
    }
}
