using MediatR;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.ProductCategories.Commands.DeleteProductCategory
{
    public class DeleteProductCategoryCommand : IRequest<Result<bool>>
    {
        public int Id { get; set; }
    }
}