using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Domain.Constants;
using Workspace_Management_System.Domain.Identity;

namespace Workspace_Management_System.Infrastructure.Persistence.SeedData
{
    public static class AdminSeeder
    {
        public static async Task SeedAsync(
            UserManager<ApplicationUser> userManager)
        {
            const string adminEmail = "admin@workspace.com";
            const string adminPassword = "Admin@12345";

            var admin = await userManager.FindByEmailAsync(adminEmail);

            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(
                    admin,
                    adminPassword
                );

                if (!result.Succeeded)
                {
                    throw new Exception(
                        string.Join(
                            ", ",
                            result.Errors.Select(e => e.Description)
                        )
                    );
                }
            }

            if (!await userManager.IsInRoleAsync(admin, Roles.Admin))
            {
                await userManager.AddToRoleAsync(
                    admin,
                    Roles.Admin
                );
            }
        }
    }
}
