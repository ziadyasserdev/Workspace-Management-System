using MediatR;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Domain.Enums;

namespace Workspace_Management_System.Application.Features.Packages.Commands.CreatePackage;

public class CreatePackageCommand : IRequest<Result<int>>
{
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public PackageType PackageType { get; set; }

    public decimal TotalHours { get; set; }

    public int? DurationDays { get; set; }

    public decimal Price { get; set; }
}