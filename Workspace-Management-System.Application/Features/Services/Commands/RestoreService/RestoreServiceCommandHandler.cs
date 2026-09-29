using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;

namespace Workspace_Management_System.Application.Features.Services.Commands.RestoreService
{
    public class RestoreServiceCommandHandler : IRequestHandler<RestoreServiceCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public RestoreServiceCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<bool>> Handle(
            RestoreServiceCommand request,
            CancellationToken cancellationToken)
        {
            var service = await _unitOfWork.Services
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && x.IsDeleted,
                    cancellationToken);

            if (service == null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    "Deleted service not found.");
            }

            var duplicateName = await _unitOfWork.Services
                .Query()
                .AnyAsync(
                    x => x.Id != request.Id &&
                         x.Name == service.Name &&
                         !x.IsDeleted,
                    cancellationToken);

            if (duplicateName)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "An active service with the same name already exists.");
            }

            var now = DateTime.UtcNow;

            service.IsDeleted = false;
            service.IsDeletedBy = null;
            service.IsActive = true;
            service.UpdatedAt = now;
            service.UpdatedBy = _currentUser.UserId;

            _unitOfWork.Services.Update(service);

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                "Service restored successfully.");
        }
    }
}
