
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.Services.Dtos;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Services.Queries.GetServiceById;

public class GetServiceByIdQueryHandler
    : IRequestHandler<GetServiceByIdQuery, Result<ServiceEditDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public GetServiceByIdQueryHandler(
        IUnitOfWork unitOfWork,
        IStringLocalizer<SharedResources> localizer)
    {
        _unitOfWork = unitOfWork;
        _localizer = localizer;
    }

    public async Task<Result<ServiceEditDto>> Handle(
        GetServiceByIdQuery request,
        CancellationToken cancellationToken)
    {
        var service = await _unitOfWork.Services
            .Query()
            .AsNoTracking()
            .Where(x => x.Id == request.Id && !x.IsDeleted)
            .Select(x => new ServiceEditDto
            {
                Id = x.Id,
                NameEn = x.NameEn,
                NameAr = x.NameAr,
                DescriptionEn = x.DescriptionEn,
                DescriptionAr = x.DescriptionAr,
                Price = x.Price,
                IsActive = x.IsActive
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (service is null)
        {
            return Result<ServiceEditDto>.Failure(
                ResultStatus.NotFound,
                _localizer["ServiceNotFound"]);
        }

        return Result<ServiceEditDto>.Success(
            service,
            _localizer["ServiceRetrievedSuccessfully"]);
    }
}
