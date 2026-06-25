using FluentValidation;
using Lararium.Authorization.Jwt.Models;
using Lararium.Authorization.Jwt.Services;
using Lararium.Authorization.Jwt.Validators;
using Lararium.Core;
using Lararium.Core.Modules;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Lararium.Authorization.Jwt;

public sealed class ModuleInitializer : IModuleInitializer
{
    private readonly ModuleMetadata _metadata = new(
        id: Guid.Parse("1d5da614-f64d-4795-bae9-c26a9de4827f"),
        name: "Lararium JWT Authorization",
        assembly: typeof(ModuleInitializer).Assembly,
        version: "1.0.0",
        priority: 1,
        hasApiControllers: true
    );

    public ModuleMetadata GetMetadata()
    {
        return _metadata;
    }

    public IServiceCollection AddServices(IServiceCollection services, IConfiguration configuration)
    {
        RegisterOptions(services, configuration);

        services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>(includeInternalTypes: true);

        services.AddScoped<IPasswordHasher<LarariumUser>, PasswordHasher<LarariumUser>>();
        services.AddScoped<JwtTokenService>();
        services.AddScoped<IJwtIdentityService, JwtIdentityService>();

        return ConfigurateAuthentication(services, configuration);
    }

    private IServiceCollection RegisterOptions(IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<JwtOptions>()
            .Bind(configuration.GetSection("Jwt"))
            .ValidateOnStart();

        return services;
    }

    private IServiceCollection ConfigurateAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        var jwtKey = configuration.GetSection("Jwt")["Key"];

        ArgumentException.ThrowIfNullOrEmpty(jwtKey);

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!)),
            ValidateIssuer = false,
            ValidateAudience = false,
            RequireExpirationTime = true,
            ClockSkew = TimeSpan.Zero,
            ValidateLifetime = true,
        };

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }
        ).AddJwtBearer(
            JwtBearerDefaults.AuthenticationScheme,
            options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = tokenValidationParameters;
            }
        );

        return services;
    }
}
