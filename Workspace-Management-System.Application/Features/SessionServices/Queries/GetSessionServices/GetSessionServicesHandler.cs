using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.PaginatedResults;
using Workspace_Management_System.Application.Contracts;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Sessions.Services.Dtos;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Sessions.Services.Queries.GetSessionServices;

public class GetSessionServicesHandler
    : IRequestHandler<
        GetSessionServicesQuery,
        PaginatedResult<SessionServiceResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILocalizationService _localizationService;
    private readonly IStringLocalizer _localizer;

    public GetSessionServicesHandler(
        IUnitOfWork unitOfWork,
        ILocalizationService localizationService,
        IStringLocalizerFactory factory)
    {
        _unitOfWork = unitOfWork;
        _localizationService = localizationService;
        _localizer = factory.Create(typeof(SharedResources));
    }

    public async Task<PaginatedResult<SessionServiceResponseDto>> Handle(
        GetSessionServicesQuery request,
        CancellationToken cancellationToken)
    {
        var session = await _unitOfWork.Sessions
            .Query()
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == request.SessionId,
                cancellationToken);

        if (session == null)
        {
            throw new KeyNotFoundException(
                _localizer["SessionNotFound"]);
        }

        var query = _unitOfWork.SessionServices
            .Query()
            .AsNoTracking()
            .Include(x => x.Service)
            .Where(x => x.SessionId == request.SessionId);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();

            query = query.Where(x =>
                x.ServiceId.ToString().Contains(search) ||
                x.Service.NameEn.Contains(search) ||
                x.Service.NameAr.Contains(search));
        }

        var totalCount = await query.CountAsync(
            cancellationToken);

        var sessionServices = await query
            .OrderBy(x => x.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var items = sessionServices
            .Select(x => new SessionServiceResponseDto
            {
                Id = x.Id,
                SessionId = x.SessionId,
                ServiceId = x.ServiceId,
                ServiceName = _localizationService.GetLocalizedValue(
                    x.Service.NameEn,
                    x.Service.NameAr),
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice,
                TotalPrice = x.Quantity * x.UnitPrice
            })
            .ToList();

        return new PaginatedResult<SessionServiceResponseDto>(
            items,
            totalCount,
            request.PageNumber,
            request.PageSize);
    }
}