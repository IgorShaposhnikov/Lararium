using Microsoft.OpenApi;

namespace Lararium.API.Extensions;

internal static class AuthSwaggerConfigurationExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddSwaggerJwtAuthorization()
        {
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("bearer", document)] = []
                });
            });

            return services;
        }
    }
}
