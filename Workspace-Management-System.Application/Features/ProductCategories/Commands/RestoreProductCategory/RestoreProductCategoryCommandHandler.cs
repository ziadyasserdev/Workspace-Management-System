using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;

namespace Workspace_Management_System.Application.Features.ProductCategories.Commands.RestoreProductCategory
{
    public class RestoreProductCategoryCommandHandler
        : IRequestHandler<RestoreProductCategoryCommand, Result<bool>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUser;

        public RestoreProductCategoryCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            this.unitOfWork = unitOfWork;
            this.currentUser = currentUser;
        }

        public async Task<Result<bool>> Handle(
            RestoreProductCategoryCommand request,
            CancellationToken cancellationToken)
        {
            var category = await unitOfWork.ProductCategories
                .Query()
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id,
                    cancellationToken);

            if (category == null)
            {
                return Result<bool>.Failure(
                    ResultStatus.NotFound,
                    "Product category not found.");
            }

            if (!category.IsDeleted)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "Product category is not deleted.");
            }

            var duplicate = await unitOfWork.ProductCategories
                .Query()
                .IgnoreQueryFilters()
                .AnyAsync(
                    x => x.Id != request.Id
                         && x.Name.ToLower() == category.Name.ToLower()
                         && !x.IsDeleted,
                    cancellationToken);

            if (duplicate)
            {
                return Result<bool>.Failure(
                    ResultStatus.Conflict,
                    "Cannot restore category because another active category has the same name.");
            }

            category.IsDeleted = false;
            category.IsActive = false;

            category.UpdatedAt = DateTime.UtcNow;
            category.UpdatedBy = currentUser.UserId;

            await unitOfWork.SaveAsync();

            return Result<bool>.Success(true,
                "Product category restored successfully.");
        }
    }
}