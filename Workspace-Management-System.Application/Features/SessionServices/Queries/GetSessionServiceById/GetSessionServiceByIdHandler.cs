using MediatR;
using Workspace_Management_System.Application.Contracts;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Sessions.Services.Dtos;

namespace Workspace_Management_System.Application.Features.Sessions.Services.Queries.GetSessionServiceById;

public class GetSessionServiceByIdHandler
    : IRequestHandler<GetSessionServiceByIdQuery, SessionServiceResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;

    public GetSessionServiceByIdHandler(
        IUnitOfWork unitOfWork,
        ILocalizationService localizationService)
    {
        _unitOfWork = unitOfWork;
        _localizationService = localizationService;
    }

    public async Task<SessionServiceResponseDto> Handle(
        GetSessionServiceByIdQuery request,
        CancellationToken cancellationToken)
    {
        var sessionService = await _unitOfWork.SessionServices
            .GetByIdAsync(request.Id);

        if (sessionService == null)
            throw new KeyNotFoundException("Session service not found.");

        var service = await _unitOfWork.Services
            .GetByIdAsync(sessionService.ServiceId);

        if (service == null)
            throw new KeyNotFoundException("Service not found.");

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