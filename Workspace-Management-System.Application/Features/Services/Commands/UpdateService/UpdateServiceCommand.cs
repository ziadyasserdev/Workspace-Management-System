using MediatR;
using Workspace_Management_System.Application.Common.Results;

namespace Workspace_Management_System.Application.Features.Services.Commands.UpdateService
{
    public class UpdateServiceCommand : IRequest<Result<bool>>
    {
        public int Id { get; set; }
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
    }
}