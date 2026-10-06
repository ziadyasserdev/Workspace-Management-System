using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.PaginatedResults;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Services.Dtos;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Services.Queries.GetServices;

public class GetServicesQueryHandler
    : IRequestHandler<
        GetServicesQuery,
        Result<PaginatedResult<ServiceResponseDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public GetServicesQueryHandler(
        IUnitOfWork unitOfWork,
        IStringLocalizer<SharedResources> localizer)
    {
        _unitOfWork = unitOfWork;
        _localizer = localizer;
    }

    public async Task<Result<PaginatedResult<ServiceResponseDto>>> Handle(
        GetServicesQuery request,
        CancellationToken cancellationToken)
    {
        var query = _unitOfWork.Services
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

        if (request.IsActive.HasValue)
        {
            query = query.Where(x =>
                x.IsActive == request.IsActive.Value);
        }

        var totalCount = await query.CountAsync(
            cancellationToken);

        var items = await query
            .OrderBy(x => x.NameEn)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new ServiceResponseDto
            {
                Id = x.Id,
                NameEn = x.NameEn,
                NameAr = x.NameAr,
                DescriptionEn = x.DescriptionEn,
                DescriptionAr = x.DescriptionAr,
                Price = x.Price,
                IsActive = x.IsActive
            })
            .ToListAsync(cancellationToken);

        var result = new PaginatedResult<ServiceResponseDto>(
            items,
            request.PageNumber,
            request.PageSize,
            totalCount);

        return Result<PaginatedResult<ServiceResponseDto>>.Success(
            result,
            _localizer["ServicesRetrievedSuccessfully"]);
    }
}