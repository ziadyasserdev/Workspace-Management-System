using MediatR;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Packages.Commands.UpdatePackage;

public class UpdatePackageCommand : IRequest<Result<int>>
{
    public int Id { get; set; }

    public string NameEn { get; set; } = null!;

    public string NameAr { get; set; } = null!;

    public string? DescriptionEn { get; set; }

    public string? DescriptionAr { get; set; }

    public PackageType PackageType { get; set; }

    public decimal TotalHours { get; set; }

    public int? DurationDays { get; set; }

    public decimal Price { get; set; }

    public bool IsActive { get; set; }
}