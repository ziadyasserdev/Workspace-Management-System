using MediatR;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.ProductCategories.Commands.UpdateProductCategory
{
    public class UpdateProductCategoryCommand : IRequest<Result<bool>>
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public bool IsActive { get; set; }
    }
}