using MediatR;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Contracts;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Sessions.Services.Dtos;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Sessions.Services.Queries.GetSessionServiceById;

public class GetSessionServiceByIdHandler
    : IRequestHandler<GetSessionServiceByIdQuery, SessionServiceResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public GetSessionServiceByIdHandler(
        IUnitOfWork unitOfWork,
        ILocalizationService localizationService,
        IStringLocalizer<SharedResources> localizer)
    {
        _unitOfWork = unitOfWork;
        _localizationService = localizationService;
        _localizer = localizer;
    }

    public async Task<SessionServiceResponseDto> Handle(
        GetSessionServiceByIdQuery request,
        CancellationToken cancellationToken)
    {
        var sessionService = await _unitOfWork.SessionServices
            .GetByIdAsync(request.Id);

        if (sessionService == null)
            throw new KeyNotFoundException(
                _localizer["SessionServiceNotFound"]);

        var service = await _unitOfWork.Services
            .GetByIdAsync(sessionService.ServiceId);

        if (service == null)
            throw new KeyNotFoundException(
                _localizer["ServiceNotFound"]);

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