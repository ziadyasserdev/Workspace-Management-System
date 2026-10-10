
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.PricingRules.Dtos;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.PricingRule.Queries.GetPricingRuleById;

public class GetPricingRuleByIdQueryHandler
    : IRequestHandler<GetPricingRuleByIdQuery, Result<PricingRuleEditDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer _localizer;

    public GetPricingRuleByIdQueryHandler(
        IUnitOfWork unitOfWork,
        IStringLocalizerFactory factory)
    {
        _unitOfWork = unitOfWork;
        _localizer = factory.Create(typeof(SharedResources));
    }

    public async Task<Result<PricingRuleEditDto>> Handle(
        GetPricingRuleByIdQuery request,
        CancellationToken cancellationToken)
    {
        var pricingRule = await _unitOfWork.PricingRules
            .Query()
            .AsNoTracking()
            .Where(x => x.Id == request.Id && !x.IsDeleted)
            .Select(x => new PricingRuleEditDto
            {
                Id = x.Id,
                PricingPlanId = x.PricingPlanId,
                WorkspaceTypeId = x.WorkspaceTypeId,
                RuleType = x.RuleType.ToString(),
                Value = x.Value,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                DayOfWeek = x.DayOfWeek.HasValue
                    ? x.DayOfWeek.Value.ToString()
                    : null,
                IsActive = x.IsActive
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (pricingRule is null)
        {
            return Result<PricingRuleEditDto>.Failure(
                ResultStatus.NotFound,
                _localizer["PricingRuleNotFound"]);
        }

        return Result<PricingRuleEditDto>.Success(
            pricingRule,
            _localizer["PricingRuleRetrievedSuccessfully"]);
    }
}
