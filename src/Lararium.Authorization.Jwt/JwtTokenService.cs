using Lararium.Authorization.Jwt.Models;
using Lararium.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Lararium.Authorization.Jwt;

public sealed class JwtTokenService(IPasswordHasher<LarariumUser> passwordHasher, IOptions<JwtOptions> jwtOptions)
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;
    private readonly JsonWebTokenHandler _tokenHandler = new();

    public RefreshToken GenerateRefreshToken()
    {
        return new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expires = DateTime.UtcNow.Add(_jwtOptions.RefreshTokenExpiration),
            Created = DateTime.UtcNow,
        };
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(_jwtOptions.AccessTokenExpiration),
            SigningCredentials = creds
        };

        return _tokenHandler.CreateToken(tokenDescriptor);
    }

    public async Task<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key)),
            ValidateIssuer = _jwtOptions.ValidateIssuer,
            ValidIssuer = _jwtOptions.ValidateIssuer ? _jwtOptions.Issuer : null,
            ValidateAudience = _jwtOptions.ValidateAudience,
            ValidAudience = _jwtOptions.ValidateAudience ? _jwtOptions.Audience : null,
            ValidateLifetime = false,
            ClockSkew = TimeSpan.Zero
        };

        var result = await _tokenHandler.ValidateTokenAsync(token, tokenValidationParameters);

        if (!result.IsValid)
        {
            throw new SecurityTokenException("Token validation failed", result.Exception);
        }

        if (result.SecurityToken is not JsonWebToken jwtToken)
        {
            throw new SecurityTokenException("Invalid token type");
        }

        var alg = jwtToken.Alg;

        if (!alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase) &&
            !alg.Equals(SecurityAlgorithms.HmacSha512Signature, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException($"Unsupported algorithm: {alg}");
        }

        return new ClaimsPrincipal(result.ClaimsIdentity);
    }

    public bool IsPasswordCorrect(string password, string hash, LarariumUser? user = null)
    {
        return passwordHasher.VerifyHashedPassword(user!, hash, password) == PasswordVerificationResult.Success;
    }

    public string GenerateHashFromString(string password, LarariumUser? user = null)
    {
        return passwordHasher.HashPassword(user!, password);
    }
}
