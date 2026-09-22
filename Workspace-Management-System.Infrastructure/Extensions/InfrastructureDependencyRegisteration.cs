using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Contracts.ExternalServices;
using Workspace_Management_System.Application.Contracts.Identity;
using Workspace_Management_System.Application.Contracts.Repositories;
using Workspace_Management_System.Application.Contracts.Services;
using Workspace_Management_System.Domain.Identity;
using Workspace_Management_System.Infrastructure.ExternalServices;
using Workspace_Management_System.Infrastructure.Identity;
using Workspace_Management_System.Infrastructure.Persistence.Context;
using Workspace_Management_System.Infrastructure.Repositories;
using Workspace_Management_System.Infrastructure.Services;

namespace Workspace_Management_System.Infrastructure.Extensions
{
    public static class InfrastructureDependencyRegisteration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
             options.UseSqlServer(configuration.GetConnectionString("DbConn")));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IEmailService, EmailService>();
      services.AddScoped<IAuthService, AuthService>();
            services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
           ;
            return services;
        }
    }
}
