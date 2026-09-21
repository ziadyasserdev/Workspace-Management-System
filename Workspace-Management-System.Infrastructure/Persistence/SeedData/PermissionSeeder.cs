using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Domain.Constants;

namespace Workspace_Management_System.Infrastructure.Persistence.SeedData
{
    public static class RolePermissionSeeder
    {
        public static async Task SeedAsync(
            RoleManager<IdentityRole> roleManager)
        {
            var rolePermissions = new Dictionary<string, string[]>
            {
                [Roles.Admin] =
                [
                    Permissions.CustomersView,
                    Permissions.CustomersCreate,
                    Permissions.CustomersUpdate,
                    Permissions.CustomersDelete,

                    Permissions.SessionsView,
                    Permissions.SessionsCreate,
                    Permissions.SessionsUpdate,
                    Permissions.SessionsCheckout
                ],

                [Roles.Manager] =
                [
                    Permissions.CustomersView,
                    Permissions.CustomersCreate,
                    Permissions.CustomersUpdate,

                    Permissions.SessionsView,
                    Permissions.SessionsCreate,
                    Permissions.SessionsUpdate,
                    Permissions.SessionsCheckout
                ],

                [Roles.Receptionist] =
                [
                    Permissions.CustomersView,
                    Permissions.CustomersCreate,

                    Permissions.SessionsView,
                    Permissions.SessionsCreate,
                    Permissions.SessionsUpdate,
                    Permissions.SessionsCheckout
                ]
            };

            foreach (var rolePermission in rolePermissions)
            {
                var role = await roleManager.FindByNameAsync(rolePermission.Key);

                if (role is null)
                    continue;

                var existingClaims = await roleManager.GetClaimsAsync(role);

                foreach (var permission in rolePermission.Value)
                {
                    var exists = existingClaims.Any(c =>
                        c.Type == "permission" &&
                        c.Value == permission);

                    if (!exists)
                    {
                        await roleManager.AddClaimAsync(
                            role,
                            new Claim("permission", permission));
                    }
                }
            }
        }
    }
}
