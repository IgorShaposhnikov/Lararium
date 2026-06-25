using Lararium.Authorization.Jwt.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Lararium.Authorization.Jwt.Extensions;

public static class JwtAuthenticationExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddJwtAuthorization(IConfiguration configuration)
        {
            var jwtSection = configuration.GetSection("Jwt");
            var jwtOptions = jwtSection.Get<JwtOptions>();

            if (jwtOptions == null || string.IsNullOrEmpty(jwtOptions.Key))
            {
                throw new InvalidOperationException("JWT configuration 'Key' is missing or invalid.");
            }

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                ValidateIssuer = jwtOptions.ValidateIssuer,
                ValidIssuer = jwtOptions.ValidateIssuer ? jwtOptions.Issuer : null,
                ValidateAudience = jwtOptions.ValidateAudience,
                ValidAudience = jwtOptions.ValidateAudience ? jwtOptions.Audience : null,
                ValidateLifetime = jwtOptions.ValidateLifetime,
                RequireExpirationTime = true,
                ClockSkew = TimeSpan.FromSeconds(jwtOptions.ClockSkewSeconds)
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = tokenValidationParameters;
            });

            return services;
        }
    }
}
