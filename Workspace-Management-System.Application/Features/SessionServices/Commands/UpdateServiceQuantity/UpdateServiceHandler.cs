using MediatR;
using Workspace_Management_System.Application.Contracts;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Sessions.Services.Dtos;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Sessions.Services.Commands.UpdateService;

public class UpdateServiceHandler : IRequestHandler<UpdateServiceCommand, SessionServiceResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public UpdateServiceHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<SessionServiceResponseDto> Handle(
        UpdateServiceCommand request,
        CancellationToken cancellationToken)
    {
        var session = await _unitOfWork.Sessions
            .GetByIdAsync(request.SessionId);

        if (session == null)
            throw new KeyNotFoundException("Session not found.");

        if (session.Status != SessionStatus.Active)
            throw new InvalidOperationException(
                "Service can only be updated in an active session.");

        if (request.Quantity <= 0)
            throw new ArgumentException(
                "Quantity must be greater than zero.");

        var sessionService = await _unitOfWork.SessionServices
            .GetBySessionAndServiceAsync(
                request.SessionId,
                request.ServiceId);

        if (sessionService == null)
            throw new KeyNotFoundException(
                "Service is not added to this session.");

        sessionService.Quantity = request.Quantity;
        sessionService.UpdatedAt = DateTime.UtcNow;
        sessionService.UpdatedBy = _currentUserService.UserId;

        _unitOfWork.SessionServices.Update(sessionService);

        await _unitOfWork.SaveAsync();

        return new SessionServiceResponseDto
        {
            Id = sessionService.Id,
            SessionId = sessionService.SessionId,
            ServiceId = sessionService.ServiceId,
            ServiceName = sessionService.Service.Name,
            Quantity = sessionService.Quantity,
            UnitPrice = sessionService.UnitPrice,
            TotalPrice = sessionService.Quantity * sessionService.UnitPrice,
            CreatedAt = sessionService.CreatedAt,
            CreatedBy = sessionService.CreatedBy,
            UpdatedAt = sessionService.UpdatedAt,
            UpdatedBy = sessionService.UpdatedBy
        };
    }
}