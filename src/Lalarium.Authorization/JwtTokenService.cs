using Lararium.Authorization.Jwt.Models;
using Lararium.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Lararium.Authorization.Jwt
{
    public class JwtTokenService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly HashAlgorithmName _hashAlgorithmName = HashAlgorithmName.SHA512;
        private readonly IPasswordHasher<LarariumUser> _passwordHasher;

        public JwtTokenService(IPasswordHasher<LarariumUser> passwordHasher,  IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
            _passwordHasher = passwordHasher;
        }

        public RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.UtcNow.Add(_jwtOptions.RefreshTokenExpiration),
                Created = DateTime.UtcNow,
            };

            return refreshToken;
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtOptions.Key)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.Add(_jwtOptions.AccessTokenExpiration),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().
                WriteToken(token);
        }

        public bool IsPasswordCorrect(string password, string hash, LarariumUser user = null)
        {
            return _passwordHasher.VerifyHashedPassword(null, hash, password) == PasswordVerificationResult.Success;
        }

        public string GenerateHashFromString(string password, LarariumUser user = null)
        {
            return _passwordHasher.HashPassword(user, password);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(_jwtOptions.Key)),
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

                var jwtToken = tokenHandler.ReadJwtToken(token);

                // Проверяем алгоритм из заголовка
                var alg = jwtToken.Header.Alg;

                if (!alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase) &&
                    !alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase) &&
                    !alg.Equals(SecurityAlgorithms.HmacSha256Signature, StringComparison.InvariantCultureIgnoreCase) &&
                    !alg.Equals(SecurityAlgorithms.HmacSha512Signature, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException($"Unsupported algorithm: {alg}");
                }

                return principal;
            }
            catch (Exception ex)
            {
                throw new SecurityTokenException("Token validation failed", ex);
            }
        }
    }
}
