using MediatR;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.ProductCategories.Commands.CreateProductCategory
{
    public class CreateProductCategoryCommand : IRequest<Result<int>>
    {
        public string NameEn { get; set; } = null!;

        public string NameAr { get; set; } = null!;

        public string? DescriptionEn { get; set; }

        public string? DescriptionAr { get; set; }

        public bool IsActive { get; set; } = true;
    }
}