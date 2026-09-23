using MediatR;
using Workspace_Management_System.Application.Common.Results;

public class CreatePricingPlanCommand : IRequest<Result<int>>
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}