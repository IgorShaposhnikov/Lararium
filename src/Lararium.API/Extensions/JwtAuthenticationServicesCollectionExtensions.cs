using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;

namespace Lararium.API.Extensions;

internal static class JwtAuthenticationServicesCollectionExtensions
{
    extension(IServiceCollection services)
    {
        internal IServiceCollection AddJwtAuthorization(IConfiguration configuration)
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
}
