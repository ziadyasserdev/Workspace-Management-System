using MediatR;
using Workspace_Management_System.Application.Common.PaginatedResults;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Sessions.Services.Dtos;

namespace Workspace_Management_System.Application.Features.Sessions.Services.Queries.GetSessionServices;

public class GetSessionServicesHandler
    : IRequestHandler<GetSessionServicesQuery, PaginatedResult<SessionServiceResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetSessionServicesHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PaginatedResult<SessionServiceResponseDto>> Handle(
        GetSessionServicesQuery request,
        CancellationToken cancellationToken)
    {
        var session = await _unitOfWork.Sessions
            .GetByIdAsync(request.SessionId);

        if (session == null)
            throw new KeyNotFoundException("Session not found.");

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

        var items = query
            .OrderBy(x => x.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new SessionServiceResponseDto
            {
                Id = x.Id,
                SessionId = x.SessionId,
                ServiceName = x.Service.Name,
                ServiceId = x.ServiceId,
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