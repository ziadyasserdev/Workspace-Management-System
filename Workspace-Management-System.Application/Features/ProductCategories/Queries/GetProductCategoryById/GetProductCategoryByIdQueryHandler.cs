using MediatR;
using Microsoft.EntityFrameworkCore;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Features.ProductCategories.DTOs;

namespace Workspace_Management_System.Application.Features.ProductCategories.Queries.GetProductCategoryById
{
    public class GetProductCategoryByIdQueryHandler
        : IRequestHandler<
            GetProductCategoryByIdQuery,
            Result<ProductCategoryDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetProductCategoryByIdQueryHandler(
            IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<ProductCategoryDto>> Handle(
            GetProductCategoryByIdQuery request,
            CancellationToken cancellationToken)
        {
            var category = await unitOfWork.ProductCategories
                .Query()
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.IsDeleted,
                    cancellationToken);

            if (category == null)
            {
                return Result<ProductCategoryDto>.Failure(
                    ResultStatus.NotFound,
                    "Product category not found.");
            }

            var dto = new ProductCategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                IsActive = category.IsActive,
                IsDeleted = category.IsDeleted
            };

            return Result<ProductCategoryDto>.Success(
                dto,
                "Product category retrieved successfully.");
        }
    }
}