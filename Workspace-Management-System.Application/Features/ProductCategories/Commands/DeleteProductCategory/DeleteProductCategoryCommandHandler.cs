using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;

namespace Workspace_Management_System.Application.Features.ProductCategories.Commands.DeleteProductCategory
{
    public class DeleteProductCategoryCommandHandler
        : IRequestHandler<DeleteProductCategoryCommand, Result<bool>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUser;

        public DeleteProductCategoryCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            this.unitOfWork = unitOfWork;
            this.currentUser = currentUser;
        }

        public async Task<Result<bool>> Handle(
            DeleteProductCategoryCommand request,
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

            category.IsDeleted = true;
            category.IsActive = false;

            category.UpdatedAt = DateTime.UtcNow;
            category.UpdatedBy = currentUser.UserId;

            await unitOfWork.SaveAsync();

            return Result<bool>.Success(
                true,
                "Product category deleted successfully.");
        }
    }
}