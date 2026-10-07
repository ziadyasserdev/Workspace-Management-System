using MediatR;
using Microsoft.AspNetCore.Identity;
using Workspace_Management_System.Application.Common.Results;
using Workspace_Management_System.Domain.Constants;

namespace Workspace_Management_System.Application.Features.Authentications.Queries.GetRolePermissions
{
    public class GetRolePermissionsQueryHandler
        : IRequestHandler<
            GetRolePermissionsQuery,
            Result<GetRolePermissionsResponse>>
    {
        private const string PermissionClaimType = "permission";

        private readonly RoleManager<IdentityRole> roleManager;

        public GetRolePermissionsQueryHandler(
            RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        public async Task<Result<GetRolePermissionsResponse>> Handle(
            GetRolePermissionsQuery request,
            CancellationToken cancellationToken)
        {
            var roleNames = new[]
            {
                Roles.Owner,
                Roles.Admin,
                Roles.Manager,
                Roles.Receptionist,
                Roles.Cashier
            };

            var response = new GetRolePermissionsResponse();

            foreach (var roleName in roleNames)
            {
                var role = await roleManager.FindByNameAsync(roleName);

                if (role is null)
                    continue;

                var claims = await roleManager.GetClaimsAsync(role);

                var permissions = claims
                    .Where(x => x.Type == PermissionClaimType)
                    .Select(x => x.Value)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList();

                response.Roles.Add(new RolePermissionResponse
                {
                    Role = roleName,
                    Permissions = permissions
                });
            }

            return Result<GetRolePermissionsResponse>.Success(response);
        }
    }
}