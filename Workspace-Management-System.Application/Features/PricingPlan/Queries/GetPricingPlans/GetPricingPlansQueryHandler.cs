using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Workspace_Management_System.Application.Common.PaginatedResults;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.PricingPlan.Queries;

namespace Workspace_Management_System.Application.Features.PricingPlan.Queries.GetPricingPlans;

public class GetPricingPlansQueryHandler
    : IRequestHandler<
        GetPricingPlansQuery,
        Result<PaginatedResult<PricingPlanDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPricingPlansQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PaginatedResult<PricingPlanDto>>> Handle(
        GetPricingPlansQuery request,
        CancellationToken cancellationToken)
    {
        var isArabic =
            CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "ar";

        var query = _unitOfWork.PricingPlans
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

        var pricingPlans = await query
            .OrderBy(x => isArabic ? x.NameAr : x.NameEn)
            .Select(x => new PricingPlanDto
            {
                Id = x.Id,
                Name = isArabic
                    ? x.NameAr
                    : x.NameEn,
                Description = isArabic
                    ? x.DescriptionAr
                    : x.DescriptionEn,
                IsActive = x.IsActive
            })
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var result = new PaginatedResult<PricingPlanDto>(
            pricingPlans,
            request.PageNumber,
            request.PageSize,
            totalCount);

        return Result<PaginatedResult<PricingPlanDto>>.Success(result);
    }
}
