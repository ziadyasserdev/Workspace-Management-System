using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Domain.Constants;

namespace Workspace_Management_System.Infrastructure.Persistence.SeedData
{
    public static class RoleSeeder
    {
        public static async Task SeedAsync(
            RoleManager<IdentityRole> roleManager)
        {
            string[] roles =
            {
                Roles.Admin,
                Roles.Manager,
                Roles.Receptionist
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(
                        new IdentityRole(role));
                }
            }
        }
    }
}
