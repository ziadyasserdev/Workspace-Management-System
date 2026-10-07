namespace Workspace_Management_System.Application.Features.Authentications.Queries.GetRolePermissions;

public class GetRolePermissionsResponse
{
    public List<RolePermissionResponse> Roles { get; set; } = new();
}

public class RolePermissionResponse
{
    public string Role { get; set; } = string.Empty;
    public List<string> Permissions { get; set; } = new();
}