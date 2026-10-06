using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Resources;

namespace Workspace_Management_System.Application.Features.Services.Commands.DeleteService;

public class DeleteServiceCommandHandler
    : IRequestHandler<DeleteServiceCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public DeleteServiceCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser,
        IStringLocalizer<SharedResources> localizer)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _localizer = localizer;
    }

    public async Task<Result<bool>> Handle(
        DeleteServiceCommand request,
        CancellationToken cancellationToken)
    {
        var service = await _unitOfWork.Services
            .Query()
            .FirstOrDefaultAsync(
                x => x.Id == request.Id && !x.IsDeleted,
                cancellationToken);

        if (service == null)
        {
            return Result<bool>.Failure(
                ResultStatus.NotFound,
                _localizer["ServiceNotFound"]);
        }

        var now = DateTime.UtcNow;

        service.IsDeleted = true;
        service.IsActive = false;
        service.IsDeletedBy = _currentUser.UserId;
        service.UpdatedAt = now;
        service.UpdatedBy = _currentUser.UserId;

        _unitOfWork.Services.Update(service);

        await _unitOfWork.SaveAsync();

        return Result<bool>.Success(
            true,
            _localizer["ServiceDeletedSuccessfully"]);
    }
}