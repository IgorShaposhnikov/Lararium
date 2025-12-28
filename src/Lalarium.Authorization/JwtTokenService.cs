using Lararium.Authorization.Jwt.Models;
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

        public JwtTokenService(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
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
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().
                WriteToken(token);
        }

        public bool IsPasswordCorrect(string password, string hash)
        {
            return CryptographicOperations.FixedTimeEquals(
                Convert.FromBase64String(Convert.ToHexString(GenerateHashFromString(password))),
                Convert.FromBase64String(hash)
            );
        }

        public byte[] GenerateHashFromString(string value)
        {
            var salt = _jwtOptions.HashConfig.Salt;
            var iteration = _jwtOptions.HashConfig.Iteration;
            var length = _jwtOptions.HashConfig.Length;
            return Rfc2898DeriveBytes.Pbkdf2(
                value.AsSpan(),
                Convert.FromBase64String(salt!),
                iteration,
                _hashAlgorithmName,
                length
            );
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
