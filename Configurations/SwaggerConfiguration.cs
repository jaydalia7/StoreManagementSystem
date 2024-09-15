using Microsoft.OpenApi.Models;

namespace StoreManagementSystem.Configurations
{
    public static class SwaggerConfiguration
    {
        public static void AddSwaggerConfiguration(this IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. Example:\"Authorizaion: Bearer{token}\"",
                    Name = "Authorization",
                    //Type = SecuritySchemeType.ApiKey
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme{
                        Reference = new OpenApiReference{
                            Type= ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string [] {}
                    }

                });

                c.MapType<Guid>(() => new OpenApiSchema { Type = "string", Format = "uuid" });
                c.DescribeAllParametersInCamelCase();
            });
        }
    }
}
