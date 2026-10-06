using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Resources;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.SessionProducts.Commands.RemoveProduct;

public class RemoveProductHandler
    : IRequestHandler<RemoveProductCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly IStringLocalizer _localizer;

    public RemoveProductHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser,
        IStringLocalizerFactory factory)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _localizer = factory.Create(typeof(SharedResources));
    }

    public async Task<Unit> Handle(
        RemoveProductCommand request,
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
                _localizer["ProductsCanOnlyBeRemovedFromActiveSession"]);
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
            throw new KeyNotFoundException(
                _localizer["ProductNotFoundInSession"]);
        }

        sessionProduct.IsDeleted = true;
        sessionProduct.IsDeletedBy = _currentUser.UserId;
        sessionProduct.UpdatedAt = DateTime.UtcNow;
        sessionProduct.UpdatedBy = _currentUser.UserId;

        _unitOfWork.SessionProducts.Delete(sessionProduct);

        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}