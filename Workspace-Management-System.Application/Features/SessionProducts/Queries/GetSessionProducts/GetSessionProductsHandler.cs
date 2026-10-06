using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Contracts;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.SessionProducts.Dtos;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.SessionProducts.Queries.GetSessionProducts;

public class GetSessionProductsHandler
    : IRequestHandler<
        GetSessionProductsQuery,
        List<SessionProductResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public GetSessionProductsHandler(
        IUnitOfWork unitOfWork,
        ILocalizationService localizationService,
        IStringLocalizer<SharedResources> localizer)
    {
        _unitOfWork = unitOfWork;
        _localizationService = localizationService;
        _localizer = localizer;
    }

    public async Task<List<SessionProductResponseDto>> Handle(
        GetSessionProductsQuery request,
        CancellationToken cancellationToken)
    {
        var session = await _unitOfWork.Sessions
            .Query()
            .FirstOrDefaultAsync(
                x => x.Id == request.SessionId,
                cancellationToken);

        if (session is null)
        {
            throw new KeyNotFoundException(
                _localizer["SessionNotFound"]);
        }

        var sessionProducts = await _unitOfWork.SessionProducts
            .Query()
            .Include(x => x.Product)
            .Where(x => x.SessionId == request.SessionId)
            .ToListAsync(cancellationToken);

        return sessionProducts
            .Select(x => new SessionProductResponseDto
            {
                Id = x.Id,
                SessionId = x.SessionId,
                ProductId = x.ProductId,
                ProductName = _localizationService.GetLocalizedValue(
                    x.Product.NameEn,
                    x.Product.NameAr),
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice,
                Total = x.Quantity * x.UnitPrice
            })
            .ToList();
    }
}