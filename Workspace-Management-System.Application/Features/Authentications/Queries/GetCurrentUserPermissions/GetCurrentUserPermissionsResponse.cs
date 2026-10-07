namespace Workspace_Management_System.Application.Features.Authentications.Queries.GetCurrentUserPermissions;

public class GetCurrentUserPermissionsResponse
{
    public string UserId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
    public List<string> Permissions { get; set; } = new();
}