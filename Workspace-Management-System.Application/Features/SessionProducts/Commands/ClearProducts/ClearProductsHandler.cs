using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Resources;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.SessionProducts.Commands.ClearProducts;

public class ClearProductsHandler
    : IRequestHandler<ClearProductsCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public ClearProductsHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser,
        IStringLocalizer<SharedResources> localizer)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _localizer = localizer;
    }

    public async Task<Unit> Handle(
        ClearProductsCommand request,
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
                _localizer["ProductsCanOnlyBeClearedFromActiveSession"]);
        }

        var sessionProducts = await _unitOfWork.SessionProducts
            .Query()
            .Where(x => x.SessionId == request.SessionId)
            .ToListAsync(cancellationToken);

        foreach (var sessionProduct in sessionProducts)
        {
            sessionProduct.IsDeleted = true;
            sessionProduct.IsDeletedBy = _currentUser.UserId;
            sessionProduct.UpdatedAt = DateTime.UtcNow;
            sessionProduct.UpdatedBy = _currentUser.UserId;

            _unitOfWork.SessionProducts.Delete(sessionProduct);
        }

        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}