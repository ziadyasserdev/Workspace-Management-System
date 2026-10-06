using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Globalization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.PricingPlan.Queries;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.PricingPlan.Queries.GetPricingPlanById;

public class GetPricingPlanByIdQueryHandler
    : IRequestHandler<GetPricingPlanByIdQuery, Result<PricingPlanDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public GetPricingPlanByIdQueryHandler(
        IUnitOfWork unitOfWork,
        IStringLocalizer<SharedResources> localizer)
    {
        _unitOfWork = unitOfWork;
        _localizer = localizer;
    }

    public async Task<Result<PricingPlanDto>> Handle(
        GetPricingPlanByIdQuery request,
        CancellationToken cancellationToken)
    {
        var isArabic =
            CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "ar";

        var pricingPlan = await _unitOfWork.PricingPlans
            .Query()
            .AsNoTracking()
            .Where(x =>
                x.Id == request.Id &&
                !x.IsDeleted)
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
            .FirstOrDefaultAsync(cancellationToken);

        if (pricingPlan == null)
        {
            return Result<PricingPlanDto>.Failure(
                ResultStatus.NotFound,
                _localizer["PricingPlanNotFound"]);
        }

        return Result<PricingPlanDto>.Success(
            pricingPlan,
            _localizer["PricingPlanRetrievedSuccessfully"]);
    }
}
