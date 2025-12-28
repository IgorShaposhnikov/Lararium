using Lararium.Authorization.Jwt.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lararium.Authorization.Jwt
{
    public class ModuleInitializer
    {

    }

    public static class JwtAuthExtensions
    {
        extension(IServiceCollection services)
        {
            public IServiceCollection AddJwtAuthModule(IConfiguration _configuration)
            {
                services.RegisterOptions(_configuration);

                services.AddScoped<JwtTokenService>();

                return services;
            }

            internal IServiceCollection RegisterOptions(IConfiguration _configuration)
            {
                services.AddOptions<JwtOptions>()
                    .Bind(_configuration.GetSection("Jwt"))
                    .ValidateOnStart();

                return services;
            }
        }
    }
}
