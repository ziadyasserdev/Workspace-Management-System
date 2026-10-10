
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.PricingPlan.Queries;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.PricingPlan.Queries.GetPricingPlanById;

public class GetPricingPlanByIdQueryHandler
    : IRequestHandler<GetPricingPlanByIdQuery, Result<PricingPlanEditDto>>
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

    public async Task<Result<PricingPlanEditDto>> Handle(
        GetPricingPlanByIdQuery request,
        CancellationToken cancellationToken)
    {
        var pricingPlan = await _unitOfWork.PricingPlans
            .Query()
            .AsNoTracking()
            .Where(x => x.Id == request.Id && !x.IsDeleted)
            .Select(x => new PricingPlanEditDto
            {
                Id = x.Id,
                NameEn = x.NameEn,
                NameAr = x.NameAr,
                DescriptionEn = x.DescriptionEn,
                DescriptionAr = x.DescriptionAr,
                IsActive = x.IsActive
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (pricingPlan is null)
        {
            return Result<PricingPlanEditDto>.Failure(
                ResultStatus.NotFound,
                _localizer["PricingPlanNotFound"]);
        }

        return Result<PricingPlanEditDto>.Success(
            pricingPlan,
            _localizer["PricingPlanRetrievedSuccessfully"]);
    }
}
