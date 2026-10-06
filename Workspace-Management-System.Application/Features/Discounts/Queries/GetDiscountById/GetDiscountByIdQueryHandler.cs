
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Discounts.Dtos;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Discounts.Queries.GetDiscountById;

public class GetDiscountByIdQueryHandler
    : IRequestHandler<GetDiscountByIdQuery, Result<DiscountResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IStringLocalizer _localizer;

    public GetDiscountByIdQueryHandler(
        IUnitOfWork unitOfWork,
        ILocalizationService localizationService,
        IStringLocalizerFactory factory)
    {
        _unitOfWork = unitOfWork;
        _localizationService = localizationService;
        _localizer = factory.Create(typeof(SharedResources));
    }

    public async Task<Result<DiscountResponseDto>> Handle(
        GetDiscountByIdQuery request,
        CancellationToken cancellationToken)
    {
        var discount = await _unitOfWork.Discounts
            .Query()
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == request.Id && !x.IsDeleted,
                cancellationToken);

        if (discount is null)
        {
            return Result<DiscountResponseDto>.Failure(
                ResultStatus.NotFound,
                _localizer["DiscountNotFound"]);
        }

        var dto = new DiscountResponseDto
        {
            Id = discount.Id,
            Name = _localizationService.GetLocalizedValue(
                discount.NameEn,
                discount.NameAr),
            Description = _localizationService.GetLocalizedValue(
                discount.DescriptionEn,
                discount.DescriptionAr),
            Type = _localizer[
                $"DiscountType_{discount.DiscountType}"],
            Value = discount.Value,
            IsActive = discount.IsActive,
            StartDate = discount.StartDate,
            EndDate = discount.EndDate
        };

        return Result<DiscountResponseDto>.Success(
            dto,
            _localizer["DiscountRetrievedSuccessfully"]);
    }
}