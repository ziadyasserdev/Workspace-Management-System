using MediatR;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.ProductCategories.Commands.RestoreProductCategory
{
    public class RestoreProductCategoryCommand : IRequest<Result<bool>>
    {
        public int Id { get; set; }
    }
}