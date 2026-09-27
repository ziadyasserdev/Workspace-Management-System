using MediatR;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.ProductCategories.Commands.CreateProductCategory
{
    public class CreateProductCategoryCommand : IRequest<Result<int>>
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
    }
}