using MediatR;
using Microsoft.AspNetCore.Identity;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Domain.Identity;

namespace Workspace_Management_System.Application.Features.Authentications.Queries.GetCurrentUserPermissions;

public class GetCurrentUserPermissionsQueryHandler
    : IRequestHandler<
        GetCurrentUserPermissionsQuery,
        Result<GetCurrentUserPermissionsResponse>>
{
    private const string PermissionClaimType = "permission";

    private readonly ICurrentUserService currentUserService;
    private readonly UserManager<ApplicationUser> userManager;
    private readonly RoleManager<IdentityRole> roleManager;

    public GetCurrentUserPermissionsQueryHandler(
        ICurrentUserService currentUserService,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        this.currentUserService = currentUserService;
        this.userManager = userManager;
        this.roleManager = roleManager;
    }

    public async Task<Result<GetCurrentUserPermissionsResponse>> Handle(
        GetCurrentUserPermissionsQuery request,
        CancellationToken cancellationToken)
    {
        if (!currentUserService.IsAuthenticated ||
            string.IsNullOrWhiteSpace(currentUserService.UserId))
        {
            throw new UnauthorizedAccessException();
        }

        var user = await userManager.FindByIdAsync(
            currentUserService.UserId);

        if (user is null)
        {
            throw new UnauthorizedAccessException();
        }

        var roles = await userManager.GetRolesAsync(user);

        var permissions = new List<string>();

        foreach (var roleName in roles)
        {
            var role = await roleManager.FindByNameAsync(roleName);

            if (role is null)
            {
                continue;
            }

            var claims = await roleManager.GetClaimsAsync(role);

            permissions.AddRange(
                claims
                    .Where(x => x.Type == PermissionClaimType)
                    .Select(x => x.Value));
        }

        var response = new GetCurrentUserPermissionsResponse
        {
            UserId = user.Id,
            Email = user.Email ?? string.Empty,
            Roles = roles.ToList(),
            Permissions = permissions
                .Distinct()
                .OrderBy(x => x)
                .ToList()
        };

        return Result<GetCurrentUserPermissionsResponse>.Success(response);
    }
}
