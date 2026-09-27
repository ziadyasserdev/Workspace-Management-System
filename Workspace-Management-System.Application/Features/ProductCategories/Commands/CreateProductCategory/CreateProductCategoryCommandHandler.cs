using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Domain.Models;

namespace Workspace_Management_System.Application.Features.ProductCategories.Commands.CreateProductCategory
{
    public class CreateProductCategoryCommandHandler
        : IRequestHandler<CreateProductCategoryCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUser;

        public CreateProductCategoryCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            this.unitOfWork = unitOfWork;
            this.currentUser = currentUser;
        }

        public async Task<Result<int>> Handle(
            CreateProductCategoryCommand request,
            CancellationToken cancellationToken)
        {
            var name = request.Name.Trim();

            var checkExist = await unitOfWork.ProductCategories
                .Query()
                .AnyAsync(
                    x => x.Name.ToLower() == name.ToLower()
                         && !x.IsDeleted,
                    cancellationToken);

            if (checkExist)
            {
                return Result<int>.Failure(
                    ResultStatus.Conflict,
                    "A product category with the same name already exists.");
            }

            var category = new ProductCategory
            {
                Name = name,
                Description = request.Description?.Trim(),
                IsActive = request.IsActive,

                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUser.UserId
            };

            await unitOfWork.ProductCategories.AddAsync(category);

            await unitOfWork.SaveAsync();

            return Result<int>.Success(
                category.Id,
                "Product category created successfully.");
        }
    }
}