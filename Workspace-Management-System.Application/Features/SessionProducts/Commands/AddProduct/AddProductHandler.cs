using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Contracts;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.SessionProducts.Dtos;
using Workspace_Management_System.Application.Resources;
using Workspace_Management_System.Domain.Enums;
using Workspace_Management_System.Domain.Models;

namespace Workspace_Management_System.Application.Features.SessionProducts.Commands.AddProduct;

public class AddProductHandler
    : IRequestHandler<AddProductCommand, SessionProductResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly ILocalizationService _localizationService;
    private readonly IStringLocalizer _localizer;

    public AddProductHandler(
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
        AddProductCommand request,
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
                _localizer["ProductsCanOnlyBeAddedToActiveSession"]);
        }

        var product = await _unitOfWork.Products
            .Query()
            .FirstOrDefaultAsync(
                x => x.Id == request.ProductId,
                cancellationToken);

        if (product is null)
        {
            throw new KeyNotFoundException(
                _localizer["ProductNotFound"]);
        }

        var localizedProductName =
            _localizationService.GetLocalizedValue(
                product.NameEn,
                product.NameAr);

        if (!product.IsActive)
        {
            throw new InvalidOperationException(
                _localizer["ProductIsNotActive", localizedProductName]);
        }

        var sessionProduct = await _unitOfWork.SessionProducts
            .Query()
            .FirstOrDefaultAsync(
                x =>
                    x.SessionId == request.SessionId &&
                    x.ProductId == request.ProductId,
                cancellationToken);

        if (sessionProduct is null)
        {
            sessionProduct = new SessionProduct
            {
                SessionId = request.SessionId,
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                UnitPrice = product.SellingPrice,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = _currentUser.UserId
            };

            await _unitOfWork.SessionProducts
                .AddAsync(sessionProduct);
        }
        else
        {
            sessionProduct.Quantity += request.Quantity;
            sessionProduct.UpdatedAt = DateTime.UtcNow;
            sessionProduct.UpdatedBy = _currentUser.UserId;

            _unitOfWork.SessionProducts
                .Update(sessionProduct);
        }

        await _unitOfWork.SaveAsync();

        return new SessionProductResponseDto
        {
            Id = sessionProduct.Id,
            SessionId = sessionProduct.SessionId,
            ProductId = sessionProduct.ProductId,
            ProductName = localizedProductName,
            Quantity = sessionProduct.Quantity,
            UnitPrice = sessionProduct.UnitPrice,
            Total = sessionProduct.Quantity * sessionProduct.UnitPrice
        };
    }
}