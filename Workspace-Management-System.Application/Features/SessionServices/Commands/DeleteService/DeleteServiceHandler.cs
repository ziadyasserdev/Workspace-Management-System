using MediatR;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Resources;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Sessions.Services.Commands.DeleteService;

public class DeleteServiceHandler : IRequestHandler<DeleteServiceCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IStringLocalizer _localizer;

    public DeleteServiceHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IStringLocalizerFactory factory)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _localizer = factory.Create(typeof(SharedResources));
    }

    public async Task<bool> Handle(
        DeleteServiceCommand request,
        CancellationToken cancellationToken)
    {
        var session = await _unitOfWork.Sessions
            .GetByIdAsync(request.SessionId);

        if (session == null)
            throw new KeyNotFoundException(
                _localizer["SessionNotFound"]);

        if (session.Status != SessionStatus.Active)
            throw new InvalidOperationException(
                _localizer["ServiceCanOnlyBeDeletedFromActiveSession"]);

        var sessionService = await _unitOfWork.SessionServices
            .GetBySessionAndServiceAsync(
                request.SessionId,
                request.ServiceId);

        if (sessionService == null)
            throw new KeyNotFoundException(
                _localizer["ServiceNotAddedToSession"]);

        sessionService.IsDeleted = true;
        sessionService.IsDeletedBy = _currentUserService.UserId;
        sessionService.UpdatedAt = DateTime.UtcNow;
        sessionService.UpdatedBy = _currentUserService.UserId;

        _unitOfWork.SessionServices.Update(sessionService);

        await _unitOfWork.SaveAsync();

        return true;
    }
}