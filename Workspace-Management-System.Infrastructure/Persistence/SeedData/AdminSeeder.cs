using Microsoft.AspNetCore.Identity;
using Workspace_Management_System.Domain.Constants;
using Workspace_Management_System.Domain.Identity;

namespace Workspace_Management_System.Infrastructure.Persistence.SeedData
{
    public static class AdminSeeder
    {
        public static async Task SeedAsync(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            var users = new[]
            {
                new
                {
                    Email = "owner@workspace.com",
                    Password = "Owner@12345",
                    Role = Roles.Owner
                },
                new
                {
                    Email = "admin@workspace.com",
                    Password = "Admin@12345",
                    Role = Roles.Admin
                },
                new
                {
                    Email = "manager@workspace.com",
                    Password = "Manager@12345",
                    Role = Roles.Manager
                },
                new
                {
                    Email = "reception@workspace.com",
                    Password = "Reception@12345",
                    Role = Roles.Receptionist
                },
                new
                {
                    Email = "cashier@workspace.com",
                    Password = "Cashier@12345",
                    Role = Roles.Cashier
                }
            };

            foreach (var item in users)
            {
                await SeedRoleAsync(roleManager, item.Role);

                var user = await userManager.FindByEmailAsync(item.Email);

                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = item.Email,
                        Email = item.Email,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(
                        user,
                        item.Password);

                    if (!result.Succeeded)
                    {
                        throw new Exception(
                            string.Join(
                                ", ",
                                result.Errors.Select(e => e.Description)));
                    }
                }

                if (!await userManager.IsInRoleAsync(user, item.Role))
                {
                    var result = await userManager.AddToRoleAsync(
                        user,
                        item.Role);

                    if (!result.Succeeded)
                    {
                        throw new Exception(
                            string.Join(
                                ", ",
                                result.Errors.Select(e => e.Description)));
                    }
                }
            }
        }

        private static async Task SeedRoleAsync(
            RoleManager<IdentityRole> roleManager,
            string role)
        {
            if (await roleManager.RoleExistsAsync(role))
                return;

            var result = await roleManager.CreateAsync(
                new IdentityRole(role));

            if (!result.Succeeded)
            {
                throw new Exception(
                    string.Join(
                        ", ",
                        result.Errors.Select(e => e.Description)));
            }
        }
    }
}