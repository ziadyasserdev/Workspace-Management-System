using MediatR;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Contracts;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Sessions.Services.Dtos;
using Workspace_Management_System.Application.Resources;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Sessions.Services.Commands.UpdateService;

public class UpdateServiceHandler
    : IRequestHandler<UpdateServiceCommand, SessionServiceResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly ILocalizationService _localizationService;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public UpdateServiceHandler(
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
        UpdateServiceCommand request,
        CancellationToken cancellationToken)
    {
        var session = await _unitOfWork.Sessions
            .GetByIdAsync(request.SessionId);

        if (session == null)
            throw new KeyNotFoundException(
                _localizer["SessionNotFound"]);

        if (session.Status != SessionStatus.Active)
            throw new InvalidOperationException(
                _localizer["ServiceCanOnlyBeUpdatedInActiveSession"]);

        if (request.Quantity <= 0)
            throw new ArgumentException(
                _localizer["QuantityGreaterThanZero"]);

        var sessionService = await _unitOfWork.SessionServices
            .GetBySessionAndServiceAsync(
                request.SessionId,
                request.ServiceId);

        if (sessionService == null)
            throw new KeyNotFoundException(
                _localizer["ServiceNotAddedToSession"]);

        var service = await _unitOfWork.Services
            .GetByIdAsync(sessionService.ServiceId);

        if (service == null)
            throw new KeyNotFoundException(
                _localizer["ServiceNotFound"]);

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
            ServiceName = _localizationService.GetLocalizedValue(
                service.NameEn,
                service.NameAr),
            Quantity = sessionService.Quantity,
            UnitPrice = sessionService.UnitPrice,
            TotalPrice = sessionService.Quantity * sessionService.UnitPrice
        };
    }
}