using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Workspace_Management_System.Api.Swagger
{
    public class AcceptLanguageOperationFilter : IOperationFilter
    {
        public void Apply(
            OpenApiOperation operation,
            OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();

            if (operation.Parameters.Any(p =>
                p.In == ParameterLocation.Header &&
                p.Name.Equals(
                    "Accept-Language",
                    StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Accept-Language",
                In = ParameterLocation.Header,
                Required = false,
                Description = "Response language: ar or en",
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Enum = new List<Microsoft.OpenApi.Any.IOpenApiAny>
                    {
                        new Microsoft.OpenApi.Any.OpenApiString("ar"),
                        new Microsoft.OpenApi.Any.OpenApiString("en")
                    },
                    Default = new Microsoft.OpenApi.Any.OpenApiString("en")
                }
            });
        }
    }
}
