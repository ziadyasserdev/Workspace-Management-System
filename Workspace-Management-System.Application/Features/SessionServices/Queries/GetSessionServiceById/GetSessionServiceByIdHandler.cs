
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Sessions.Services.Dtos;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Sessions.Services.Queries.GetSessionServiceById;

public class GetSessionServiceByIdHandler
    : IRequestHandler<GetSessionServiceByIdQuery, SessionServiceEditDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public GetSessionServiceByIdHandler(
        IUnitOfWork unitOfWork,
        IStringLocalizer<SharedResources> localizer)
    {
        _unitOfWork = unitOfWork;
        _localizer = localizer;
    }

    public async Task<SessionServiceEditDto> Handle(
        GetSessionServiceByIdQuery request,
        CancellationToken cancellationToken)
    {
        var sessionService = await _unitOfWork.SessionServices
            .Query()
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == request.Id && !x.IsDeleted,
                cancellationToken);

        if (sessionService is null)
        {
            throw new KeyNotFoundException(
                _localizer["SessionServiceNotFound"]);
        }

        var service = await _unitOfWork.Services
            .Query()
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == sessionService.ServiceId && !x.IsDeleted,
                cancellationToken);

        if (service is null)
        {
            throw new KeyNotFoundException(
                _localizer["ServiceNotFound"]);
        }

        return new SessionServiceEditDto
        {
            Id = sessionService.Id,
            SessionId = sessionService.SessionId,
            ServiceId = sessionService.ServiceId,
            ServiceNameEn = service.NameEn,
            ServiceNameAr = service.NameAr,
            Quantity = sessionService.Quantity,
            UnitPrice = sessionService.UnitPrice,
            TotalPrice = sessionService.Quantity * sessionService.UnitPrice
        };
    }
}
