using Lararium.Authorization.Jwt.Models;
using Lararium.Authorization.Jwt.Services;
using Lararium.Core;
using Lararium.Core.Modules;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lararium.Authorization.Jwt
{
    public class ModuleInitializer : IModuleInitializer
    {
        private readonly ModuleMetadata _metadata = new()
        {
            Id = Guid.Parse("6c16fe2c-3010-49a9-b89c-0248b171455d"),
            Name = "Lararium Jwt Authorization",
            Assembly = typeof(ModuleInitializer).Assembly,
            HasApiControllers = true,
            Version = "1.0.0"
        };

        public ModuleMetadata GetMetadata()
        {
            return _metadata;
        }

        public IServiceCollection AddServices(IServiceCollection services, IConfiguration configuration)
        {
            RegisterOptions(services, configuration);

            services.AddScoped<IPasswordHasher<LarariumUser>, PasswordHasher<LarariumUser>>();
            services.AddScoped<JwtTokenService>();
            services.AddScoped<IJwtIdentityService, JwtIdentityService>();

            return services;
        }

        private IServiceCollection RegisterOptions(IServiceCollection services, IConfiguration _configuration)
        {
            services.AddOptions<JwtOptions>()
                .Bind(_configuration.GetSection("Jwt"))
                .ValidateOnStart();

            return services;
        }
    }
}
