using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Domain.Models;

namespace Workspace_Management_System.Application.Features.Services.Commands.CreateService
{
    public class CreateServiceCommandHandler
        : IRequestHandler<CreateServiceCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public CreateServiceCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<int>> Handle(
            CreateServiceCommand request,
            CancellationToken cancellationToken)
        {
            var nameEn = request.NameEn.Trim();
            var nameAr = request.NameAr.Trim();

            var exists = await _unitOfWork.Services
                .Query()
                .AnyAsync(
                    x =>
                        (x.NameEn == nameEn || x.NameAr == nameAr)
                        && !x.IsDeleted,
                    cancellationToken);

            if (exists)
            {
                return Result<int>.Failure(
                    ResultStatus.Conflict,
                    "A service with the same name already exists.");
            }

            var now = DateTime.UtcNow;

            var service = new Service
            {
                NameEn = nameEn,
                NameAr = nameAr,
                DescriptionEn = request.DescriptionEn?.Trim(),
                DescriptionAr = request.DescriptionAr?.Trim(),
                Price = request.Price,
                IsActive = request.IsActive,
                CreatedAt = now,
                CreatedBy = _currentUser.UserId
            };

            await _unitOfWork.Services.AddAsync(service);

            await _unitOfWork.SaveAsync();

            return Result<int>.Success(
                service.Id,
                "Service created successfully.");
        }
    }
}