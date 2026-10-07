using Microsoft.AspNetCore.Authorization;

namespace Workspace_Management_System.Api.Attributes
{
    public class RequirePermissionAttribute : AuthorizeAttribute
    {
        public RequirePermissionAttribute(string permission)
        {
            Policy = permission;
        }
    }
}