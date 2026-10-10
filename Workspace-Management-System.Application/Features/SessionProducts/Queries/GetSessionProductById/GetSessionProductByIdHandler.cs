using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.SessionProducts.Dtos;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.SessionProducts.Queries.GetSessionProductById;

public class GetSessionProductByIdHandler
    : IRequestHandler<GetSessionProductByIdQuery, SessionProductEditDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public GetSessionProductByIdHandler(
        IUnitOfWork unitOfWork,
        IStringLocalizer<SharedResources> localizer)
    {
        _unitOfWork = unitOfWork;
        _localizer = localizer;
    }

    public async Task<SessionProductEditDto> Handle(
        GetSessionProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        var sessionProduct = await _unitOfWork.SessionProducts
            .Query()
            .AsNoTracking()
            .Include(x => x.Product)
            .FirstOrDefaultAsync(
                x => x.Id == request.Id && !x.IsDeleted,
                cancellationToken);

        if (sessionProduct is null)
        {
            throw new KeyNotFoundException(
                _localizer["SessionProductNotFound"]);
        }

        if (sessionProduct.Product is null || sessionProduct.Product.IsDeleted)
        {
            throw new KeyNotFoundException(
                _localizer["ProductNotFound"]);
        }

        return new SessionProductEditDto
        {
            Id = sessionProduct.Id,
            SessionId = sessionProduct.SessionId,
            ProductId = sessionProduct.ProductId,
            ProductNameEn = sessionProduct.Product.NameEn,
            ProductNameAr = sessionProduct.Product.NameAr,
            Quantity = sessionProduct.Quantity,
            UnitPrice = sessionProduct.UnitPrice,
            Total = sessionProduct.Quantity * sessionProduct.UnitPrice
        };
    }
}