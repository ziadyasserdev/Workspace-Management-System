using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.PaginatedResults;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Discounts.Dtos;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Discounts.Queries.GetDiscounts;

public class GetDiscountsQueryHandler
    : IRequestHandler<
        GetDiscountsQuery,
        Result<PaginatedResult<DiscountResponseDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IStringLocalizer _localizer;

    public GetDiscountsQueryHandler(
        IUnitOfWork unitOfWork,
        ILocalizationService localizationService,
        IStringLocalizerFactory factory)
    {
        _unitOfWork = unitOfWork;
        _localizationService = localizationService;
        _localizer = factory.Create(typeof(SharedResources));
    }

    public async Task<Result<PaginatedResult<DiscountResponseDto>>> Handle(
        GetDiscountsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _unitOfWork.Discounts
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
            query = query.Where(
                x => x.IsActive == request.IsActive.Value);
        }

        var totalCount = await query.CountAsync(
            cancellationToken);

        var discounts = await query
            .OrderBy(x => x.NameEn)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var items = discounts
            .Select(x => new DiscountResponseDto
            {
                Id = x.Id,
                Name = _localizationService.GetLocalizedValue(
                    x.NameEn,
                    x.NameAr),
                Description = _localizationService.GetLocalizedValue(
                    x.DescriptionEn,
                    x.DescriptionAr),
                Type = _localizer[
                    $"DiscountType_{x.DiscountType}"],
                Value = x.Value,
                IsActive = x.IsActive,
                StartDate = x.StartDate,
                EndDate = x.EndDate
            })
            .ToList();

        var result = new PaginatedResult<DiscountResponseDto>(
            items,
            request.PageNumber,
            request.PageSize,
            totalCount);

        return Result<PaginatedResult<DiscountResponseDto>>.Success(
            result,
            _localizer["DiscountsRetrievedSuccessfully"]);
    }
}
