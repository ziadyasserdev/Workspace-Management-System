using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Workspace_Management_System.Application.Common.Behaviors;
using Workspace_Management_System.Application.Contracts.Pricing;
using Workspace_Management_System.Application.Services.Pricing;

namespace Workspace_Management_System.Application.Extensions
{

    public static class ApplicationDependencyRegisteration
    {

        public static IServiceCollection AddApplicationDependency(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddScoped<IPricingCalculator, PricingCalculator>();

            return services;
        }
    }
}
