using MediatR;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Packages.Commands.UpdatePackage;

public class UpdatePackageCommand : IRequest<Result<int>>
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public PackageType PackageType { get; set; }

    public decimal TotalHours { get; set; }

    public int? DurationDays { get; set; }

    public decimal Price { get; set; }

    public bool IsActive { get; set; }
}