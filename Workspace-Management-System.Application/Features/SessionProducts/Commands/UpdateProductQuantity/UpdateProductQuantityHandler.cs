using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Contracts;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.SessionProducts.Dtos;
using Workspace_Management_System.Application.Resources;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.SessionProducts.Commands.UpdateProductQuantity;

public class UpdateProductQuantityHandler
    : IRequestHandler<
        UpdateProductQuantityCommand,
        SessionProductResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly ILocalizationService _localizationService;
    private readonly IStringLocalizer _localizer;

    public UpdateProductQuantityHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser,
        ILocalizationService localizationService,
        IStringLocalizerFactory factory)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _localizationService = localizationService;
        _localizer = factory.Create(typeof(SharedResources));
    }

    public async Task<SessionProductResponseDto> Handle(
        UpdateProductQuantityCommand request,
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

        if (session.Status != SessionStatus.Active)
        {
            throw new InvalidOperationException(
                _localizer["ProductQuantityCanOnlyBeUpdatedForActiveSession"]);
        }

        var sessionProduct = await _unitOfWork.SessionProducts
            .Query()
            .Include(x => x.Product)
            .FirstOrDefaultAsync(
                x =>
                    x.SessionId == request.SessionId &&
                    x.ProductId == request.ProductId,
                cancellationToken);

        if (sessionProduct is null)
        {
            throw new KeyNotFoundException(
                _localizer["ProductNotFoundInSession"]);
        }

        sessionProduct.Quantity = request.Quantity;
        sessionProduct.UpdatedAt = DateTime.UtcNow;
        sessionProduct.UpdatedBy = _currentUser.UserId;

        _unitOfWork.SessionProducts.Update(sessionProduct);

        await _unitOfWork.SaveAsync();

        return new SessionProductResponseDto
        {
            Id = sessionProduct.Id,
            SessionId = sessionProduct.SessionId,
            ProductId = sessionProduct.ProductId,
            ProductName = _localizationService.GetLocalizedValue(
                sessionProduct.Product.NameEn,
                sessionProduct.Product.NameAr),
            Quantity = sessionProduct.Quantity,
            UnitPrice = sessionProduct.UnitPrice,
            Total = sessionProduct.Quantity * sessionProduct.UnitPrice
        };
    }
}