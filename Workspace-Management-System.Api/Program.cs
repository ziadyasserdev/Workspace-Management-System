using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Workspace_Management_System.Api.Middleware;
using Workspace_Management_System.Application.Extensions;
using Workspace_Management_System.Application.Settings;
using Workspace_Management_System.Infrastructure.Extensions;
namespace Workspace_Management_System.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();




            // Add Infrastructure Services , Application Services and MediatR

            builder.Services.AddInfrastructureServices(builder.Configuration)
                .AddApplicationDependency();
            // Add JwtSetting configuration to the services container
            builder.Services.Configure<JwtSetting>(
         builder.Configuration.GetSection("JwtSetting")
     );









            builder.Services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {

                    ValidateIssuerSigningKey = true,

                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = builder.Configuration["JwtSetting:Issuer"],
                    ValidAudience = builder.Configuration["JwtSetting:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.
                    Configuration["JwtSetting:SecretKey"]))
                };
            });



            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });













            builder.Services.Configure<EmailSettings>(
      builder.Configuration.GetSection("EmailSettings")
  );



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            // Add Global Exception Handling Middleware

            app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

            app.Run();
        }
    }
}
