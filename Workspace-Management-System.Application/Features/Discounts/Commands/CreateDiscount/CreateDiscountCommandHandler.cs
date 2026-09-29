using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Domain.Models;

namespace Workspace_Management_System.Application.Features.Discounts.Commands.CreateDiscount
{
    public class CreateDiscountCommandHandler
        : IRequestHandler<CreateDiscountCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public CreateDiscountCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<int>> Handle(
            CreateDiscountCommand request,
            CancellationToken cancellationToken)
        {
            var name = request.Name.Trim();

            var exists = await _unitOfWork.Discounts
                .Query()
                .AnyAsync(
                    x => x.Name == name && !x.IsDeleted,
                    cancellationToken);

            if (exists)
            {
                return Result<int>.Failure(
                    ResultStatus.Conflict,
                    "A discount with the same name already exists.");
            }

            var now = DateTime.UtcNow;

            var discount = new Discount
            {
                Name = name,
                DiscountType = request.Type,
                Value = request.Value,
                IsActive = request.IsActive,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                CreatedAt = now,
                CreatedBy = _currentUser.UserId
            };

            await _unitOfWork.Discounts.AddAsync(discount);

            await _unitOfWork.SaveAsync();

            return Result<int>.Success(
                discount.Id,
                "Discount created successfully.");
        }
    }
}