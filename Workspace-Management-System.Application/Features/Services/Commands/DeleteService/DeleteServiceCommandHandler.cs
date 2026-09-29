using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;

namespace Workspace_Management_System.Application.Features.Services.Commands.DeleteService
{
    public class DeleteServiceCommandHandler : IRequestHandler<DeleteServiceCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public DeleteServiceCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
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
                    "Service not found.");
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
                "Service deleted successfully.");
        }
    }
}