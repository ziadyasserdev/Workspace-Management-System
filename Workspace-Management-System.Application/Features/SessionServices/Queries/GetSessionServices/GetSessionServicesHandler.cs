using MediatR;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.PaginatedResults;
using Workspace_Management_System.Application.Contracts;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Sessions.Services.Dtos;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Sessions.Services.Queries.GetSessionServices;

public class GetSessionServicesHandler
    : IRequestHandler<GetSessionServicesQuery, PaginatedResult<SessionServiceResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public GetSessionServicesHandler(
        IUnitOfWork unitOfWork,
        ILocalizationService localizationService,
        IStringLocalizer<SharedResources> localizer)
    {
        _unitOfWork = unitOfWork;
        _localizationService = localizationService;
        _localizer = localizer;
    }

    public async Task<PaginatedResult<SessionServiceResponseDto>> Handle(
        GetSessionServicesQuery request,
        CancellationToken cancellationToken)
    {
        var session = await _unitOfWork.Sessions
            .GetByIdAsync(request.SessionId);

        if (session == null)
            throw new KeyNotFoundException(
                _localizer["SessionNotFound"]);

        var services = await _unitOfWork.SessionServices
            .GetAllAsync();

        var query = services
            .Where(x => x.SessionId == request.SessionId);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query.Where(x =>
                x.ServiceId.ToString().Contains(request.Search));
        }

        var totalCount = query.Count();

        var sessionServices = query
            .OrderBy(x => x.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var items = new List<SessionServiceResponseDto>();

        foreach (var sessionService in sessionServices)
        {
            var service = await _unitOfWork.Services
                .GetByIdAsync(sessionService.ServiceId);

            items.Add(new SessionServiceResponseDto
            {
                Id = sessionService.Id,
                SessionId = sessionService.SessionId,
                ServiceId = sessionService.ServiceId,
                ServiceName = service == null
                    ? string.Empty
                    : _localizationService.GetLocalizedValue(
                        service.NameEn,
                        service.NameAr),
                Quantity = sessionService.Quantity,
                UnitPrice = sessionService.UnitPrice,
                TotalPrice = sessionService.Quantity * sessionService.UnitPrice
            });
        }

        return new PaginatedResult<SessionServiceResponseDto>(
            items,
            totalCount,
            request.PageNumber,
            request.PageSize);
    }
}