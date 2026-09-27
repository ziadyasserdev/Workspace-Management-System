using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;

namespace Workspace_Management_System.Application.Features.ProductCategories.Commands.UpdateProductCategory
{
    public class UpdateProductCategoryCommandHandler
        : IRequestHandler<UpdateProductCategoryCommand, Result<bool>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUser;

        public UpdateProductCategoryCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            this.unitOfWork = unitOfWork;
            this.currentUser = currentUser;
        }

        public async Task<Result<bool>> Handle(
            UpdateProductCategoryCommand request,
            CancellationToken cancellationToken)
        {
            var category = await unitOfWork.ProductCategories
                .Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.IsDeleted,
                    cancellationToken);

            if (category == null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    "Product category not found.");
            }

            var name = request.Name.Trim();

            var duplicate = await unitOfWork.ProductCategories
                .Query()
                .AnyAsync(
                    x => x.Id != request.Id
                         && x.Name.ToLower() == name.ToLower()
                         && !x.IsDeleted,
                    cancellationToken);

            if (duplicate)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "Another product category with the same name already exists.");
            }

            category.Name = name;
            category.Description = request.Description?.Trim();
            category.IsActive = request.IsActive;

            category.UpdatedAt = DateTime.UtcNow;
            category.UpdatedBy = currentUser.UserId;

            await unitOfWork.SaveAsync();

            return Result<bool>.Success(true,
                "Product category updated successfully.");
        }
    }
}