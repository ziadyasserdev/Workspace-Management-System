
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Discounts.Dtos;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Discounts.Queries.GetDiscountById;

public class GetDiscountByIdQueryHandler
    : IRequestHandler<GetDiscountByIdQuery, Result<DiscountEditDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer _localizer;

    public GetDiscountByIdQueryHandler(
        IUnitOfWork unitOfWork,
        IStringLocalizerFactory factory)
    {
        _unitOfWork = unitOfWork;
        _localizer = factory.Create(typeof(SharedResources));
    }

    public async Task<Result<DiscountEditDto>> Handle(
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
            return Result<DiscountEditDto>.Failure(
                ResultStatus.NotFound,
                _localizer["DiscountNotFound"]);
        }

        var dto = new DiscountEditDto
        {
            Id = discount.Id,
            NameEn = discount.NameEn,
            NameAr = discount.NameAr,
            DescriptionEn = discount.DescriptionEn,
            DescriptionAr = discount.DescriptionAr,
            DiscountType = discount.DiscountType.ToString(),
            Value = discount.Value,
            IsActive = discount.IsActive,
            StartDate = discount.StartDate,
            EndDate = discount.EndDate
        };

        return Result<DiscountEditDto>.Success(
            dto,
            _localizer["DiscountRetrievedSuccessfully"]);
    }
}
