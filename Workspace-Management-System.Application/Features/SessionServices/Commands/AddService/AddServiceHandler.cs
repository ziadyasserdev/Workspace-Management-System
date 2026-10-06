using MediatR;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Contracts;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Sessions.Services.Dtos;
using Workspace_Management_System.Application.Resources;
using Workspace_Management_System.Domain.Enums;
using Workspace_Management_System.Domain.Models;

namespace Workspace_Management_System.Application.Features.Sessions.Services.Commands.AddService;

public class AddServiceHandler
    : IRequestHandler<AddServiceCommand, SessionServiceResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly ILocalizationService _localizationService;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public AddServiceHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        ILocalizationService localizationService,
        IStringLocalizer<SharedResources> localizer)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _localizationService = localizationService;
        _localizer = localizer;
    }

    public async Task<SessionServiceResponseDto> Handle(
        AddServiceCommand request,
        CancellationToken cancellationToken)
    {
        var session = await _unitOfWork.Sessions
            .GetByIdAsync(request.SessionId);

        if (session == null)
            throw new KeyNotFoundException(
                _localizer["SessionNotFound"]);

        if (session.Status != SessionStatus.Active)
            throw new InvalidOperationException(
                _localizer["ServiceCanOnlyBeAddedToActiveSession"]);

        var service = await _unitOfWork.Services
            .GetByIdAsync(request.ServiceId);

        if (service == null)
            throw new KeyNotFoundException(
                _localizer["ServiceNotFound"]);

        if (!service.IsActive)
            throw new InvalidOperationException(
                _localizer["ServiceIsNotActive"]);

        if (request.Quantity <= 0)
            throw new ArgumentException(
                _localizer["QuantityGreaterThanZero"]);

        var existingService = await _unitOfWork.SessionServices
            .GetBySessionAndServiceAsync(
                request.SessionId,
                request.ServiceId);

        if (existingService != null)
            throw new InvalidOperationException(
                _localizer["ServiceAlreadyAddedToSession"]);

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