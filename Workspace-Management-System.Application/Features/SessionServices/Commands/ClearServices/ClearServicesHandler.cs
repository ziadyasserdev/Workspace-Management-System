using MediatR;
using Workspace_Management_System.Application.Contracts;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Sessions.Services.Commands.ClearServices;

public class ClearServicesHandler : IRequestHandler<ClearServicesCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public ClearServicesHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(
        ClearServicesCommand request,
        CancellationToken cancellationToken)
    {
        var session = await _unitOfWork.Sessions
            .GetByIdAsync(request.SessionId);

        if (session == null)
            throw new KeyNotFoundException("Session not found.");

        if (session.Status != SessionStatus.Active)
            throw new InvalidOperationException(
                "Services can only be cleared from an active session.");

        var services = await _unitOfWork.SessionServices
            .GetAllAsync();

        var sessionServices = services
            .Where(x => x.SessionId == request.SessionId)
            .ToList();

        foreach (var service in sessionServices)
        {
            service.IsDeleted = true;
            service.IsDeletedBy = _currentUserService.UserId;
            service.UpdatedAt = DateTime.UtcNow;
            service.UpdatedBy = _currentUserService.UserId;

            _unitOfWork.SessionServices.Update(service);
        }

        await _unitOfWork.SaveAsync();

        return true;
    }
}