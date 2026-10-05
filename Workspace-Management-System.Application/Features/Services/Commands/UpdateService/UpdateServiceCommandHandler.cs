using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;

namespace Workspace_Management_System.Application.Features.Services.Commands.UpdateService
{
    public class UpdateServiceCommandHandler
        : IRequestHandler<UpdateServiceCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public UpdateServiceCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<bool>> Handle(
            UpdateServiceCommand request,
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

            var duplicateName = await _unitOfWork.Services
                .Query()
                .AnyAsync(
                    x =>
                        x.Id != request.Id &&
                        !x.IsDeleted &&
                        (
                            x.NameEn == request.NameEn ||
                            x.NameAr == request.NameAr
                        ),
                    cancellationToken);

            if (duplicateName)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "A service with the same name already exists.");
            }

            service.NameEn = request.NameEn.Trim();
            service.NameAr = request.NameAr.Trim();
            service.DescriptionEn = request.DescriptionEn?.Trim();
            service.DescriptionAr = request.DescriptionAr?.Trim();
            service.Price = request.Price;
            service.IsActive = request.IsActive;
            service.UpdatedAt = DateTime.UtcNow;
            service.UpdatedBy = _currentUser.UserId;

            _unitOfWork.Services.Update(service);

            await _unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                "Service updated successfully.");
        }
    }
}