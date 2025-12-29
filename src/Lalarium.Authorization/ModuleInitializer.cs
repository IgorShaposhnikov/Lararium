using Lararium.Authorization.Jwt.Models;
using Lararium.Core;
using Lararium.Core.Modules;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lararium.Authorization.Jwt
{
    public class ModuleInitializer : IModuleInitializer
    {
        public ModuleMetadata GetMetadata()
        {
            return new()
            {
                Id = Guid.Parse("6c16fe2c-3010-49a9-b89c-0248b171455d"),
                Name = "Lararium Jwt Authorization",
                Assembly = typeof(ModuleInitializer).Assembly,
                HasApiControllers = true,
                Version = "1.0.0"
            };
        }
    }

    public static class JwtAuthExtensions
    {
        extension(IServiceCollection services)
        {
            public IServiceCollection AddJwtAuthModule(IConfiguration _configuration)
            {
                services.RegisterOptions(_configuration);

                services.AddScoped<IPasswordHasher<LarariumUser>, PasswordHasher<LarariumUser>>();
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
