using Asp.Versioning;
using Microsoft.OpenApi;

namespace Lararium.API.Extensions
{
    public static class ApiVersioningServicesCollectionExtensions
    {
        extension(IServiceCollection services) 
        {
            public IServiceCollection EnableApiVersioning(double defaultApiVersion = 1.0) 
            {
                services.AddApiVersioning(options =>
                {
                    options.DefaultApiVersion = new ApiVersion(defaultApiVersion);
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.ReportApiVersions = true; // Adds API version to headers
                    options.ApiVersionReader = ApiVersionReader.Combine(
                           new UrlSegmentApiVersionReader(),
                           new QueryStringApiVersionReader("api-version"),
                           new HeaderApiVersionReader("X-Version"),
                           new MediaTypeApiVersionReader("x-version"));
                }).AddApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'VVV"; // Formats version as v1, v2
                    options.SubstituteApiVersionInUrl = true; // Replaces {version} in route
                });

                services.AddAndConfigurateSwaggerDoc();

                return services;
            }

            private IServiceCollection AddAndConfigurateSwaggerDoc()
            {
                services.AddSwaggerGen((options =>
                 {
                     options.SwaggerDoc("v1", new OpenApiInfo
                     {
                         Version = "Version 1",
                         Title = "Lararium API V1",
                     });

                     options.SwaggerDoc("v2", new OpenApiInfo
                     {
                         Version = "Version 2",
                         Title = "Lararium API V2",
                     });
                 }));
                return services;
            }
        }
    }
}
