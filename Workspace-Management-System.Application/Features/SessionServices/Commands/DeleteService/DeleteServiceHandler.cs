using MediatR;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Sessions.Services.Commands.DeleteService;

public class DeleteServiceHandler : IRequestHandler<DeleteServiceCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteServiceHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(
        DeleteServiceCommand request,
        CancellationToken cancellationToken)
    {
        var session = await _unitOfWork.Sessions
            .GetByIdAsync(request.SessionId);

        if (session == null)
            throw new KeyNotFoundException("Session not found.");

        if (session.Status != SessionStatus.Active)
            throw new InvalidOperationException(
                "Service can only be deleted from an active session.");

        var sessionService = await _unitOfWork.SessionServices
            .GetBySessionAndServiceAsync(
                request.SessionId,
                request.ServiceId);

        if (sessionService == null)
            throw new KeyNotFoundException(
                "Service is not added to this session.");

        _unitOfWork.SessionServices.Delete(sessionService);

        await _unitOfWork.SaveAsync();

        return true;
    }
}