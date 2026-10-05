using MediatR;
using Workspace_Management_System.Application.Contracts;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Sessions.Services.Dtos;
using Workspace_Management_System.Domain.Enums;
using Workspace_Management_System.Domain.Models;

namespace Workspace_Management_System.Application.Features.Sessions.Services.Commands.AddService;

public class AddServiceHandler : IRequestHandler<AddServiceCommand, SessionServiceResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly ILocalizationService _localizationService;

    public AddServiceHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, ILocalizationService localizationService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _localizationService = localizationService;
    }

    public async Task<SessionServiceResponseDto> Handle(
        AddServiceCommand request,
        CancellationToken cancellationToken)
    {
        var session = await _unitOfWork.Sessions
            .GetByIdAsync(request.SessionId);

        if (session == null)
            throw new KeyNotFoundException("Session not found.");

        if (session.Status != SessionStatus.Active)
            throw new InvalidOperationException(
                "Service can only be added to an active session.");

        var service = await _unitOfWork.Services
            .GetByIdAsync(request.ServiceId);

        if (service == null)
            throw new KeyNotFoundException("Service not found.");

        if (!service.IsActive)
            throw new InvalidOperationException("Service is not active.");

        if (request.Quantity <= 0)
            throw new ArgumentException(
                "Quantity must be greater than zero.");

        var existingService = await _unitOfWork.SessionServices
            .GetBySessionAndServiceAsync(
                request.SessionId,
                request.ServiceId);

        if (existingService != null)
            throw new InvalidOperationException(
                "Service is already added to this session.");

        var sessionService = new SessionService
        {
            SessionId = request.SessionId,
            ServiceId = request.ServiceId,
            Quantity = request.Quantity,
            UnitPrice = service.Price,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = _currentUserService.UserId
        };

        await _unitOfWork.SessionServices.AddAsync(sessionService);
        await _unitOfWork.SaveAsync();

        return new SessionServiceResponseDto
        {
            Id = sessionService.Id,
            SessionId = sessionService.SessionId,
            ServiceName = _localizationService.GetLocalizedValue(
                service.NameEn,
                service.NameAr),
            ServiceId = sessionService.ServiceId,
            Quantity = sessionService.Quantity,
            UnitPrice = sessionService.UnitPrice,
            TotalPrice = sessionService.Quantity * sessionService.UnitPrice
        };
    }
}