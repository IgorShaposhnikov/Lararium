using Lararium.Authorization.Jwt;
using Lararium.Authorization.Jwt.Exceptions;
using Lararium.Authorization.Jwt.Models;
using Lararium.Authorization.Jwt.Models.Requests;
using Lararium.Authorization.Jwt.Models.Response;
using Lararium.Core;
using Lararium.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text.Json;

namespace Lararium.API.DataProviders.Authorization
{
    public class JwtAuthorizationProvider : IJwtAuthorizationProvider
    {
        private readonly AppDbContext _dbContext;
        private readonly IDistributedCache _cache;
        private readonly JwtTokenService _tokenService;

        public JwtAuthorizationProvider(AppDbContext dbContext, IDistributedCache cache, JwtTokenService tokenService)
        {
            _dbContext = dbContext;
            _cache = cache;
            _tokenService = tokenService;
        }

        public async Task<AuthenticationResponse> LoginUserAsync(LoginRequest request, CancellationToken cancellationToken = default!)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(
                    u => u.Login == request.Login,
                    cancellationToken);

            UserNotFoundException.ThrowIfNull(user, request.Login);

            if (!_tokenService.IsPasswordCorrect(request.Password, user!.PasswordHash!, user))
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            var claims = new List<Claim> { new("Id", user.Id.ToString()) };

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            await SaveRefreshTokenToCacheAsync(refreshToken);

            return new AuthenticationResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }

        public async Task<AuthenticationResponse> RegisterUserAsync(RegisterRequest data, CancellationToken cancellationToken = default!)
        {
            var isUserWithLoginExists = await IsUserExistsAsync(data.Login, cancellationToken);

            if (isUserWithLoginExists)
            {
                throw new Exception("User already exists");
            }

            var newUser = new LarariumUser()
            {
                FirstName = data.FirstName,
                MiddleName = data.MiddleName,
                LastName = data.LastName,
                Login = data.Login,
                PasswordHash = _tokenService.GenerateHashFromString(data.Password)
            };

            await _dbContext.Users.AddAsync(newUser, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return await LoginUserAsync(new LoginRequest { Login = data.Login, Password = data.Password }, cancellationToken);
        }

        public Task<bool> IsUserExistsAsync(string login, CancellationToken cancellationToken = default)
        {
            return _dbContext.Users
                .AsNoTracking()
                .AnyAsync(x => x.Login == login, cancellationToken);
        }


        public async Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
        {
            var principles = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
            var userId = principles.FindFirstValue("Id");


            if (string.IsNullOrEmpty(userId))
            {
                throw new SecurityTokenException("Invalid AccessToken");
            }

            var storedRefreshToken = await GetRefreshTokenFromCacheAsync(request.RefreshToken);
            var isRefreshTokenValid = storedRefreshToken != null && DateTime.UtcNow <= storedRefreshToken!.Expires;
            if (!isRefreshTokenValid)
            {
                throw new SecurityTokenException("Invalid RefreshToken");
            }

            var newRefreshToken = _tokenService.GenerateRefreshToken();
            // TODO: Сделать генерацию claims с нуля, ибо можно подделать
            var newAccessToken = _tokenService.GenerateAccessToken(principles.Claims);

            await SaveRefreshTokenToCacheAsync(newRefreshToken);
            await RemoveRefreshTokenFromCacheAsync(request.RefreshToken);

            return new RefreshTokenResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }

        #region Private Methods


        private async Task SaveRefreshTokenToCacheAsync(RefreshToken token)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = token.Expires
            };

            var refreshTokenToBytes = JsonSerializer.SerializeToUtf8Bytes(token);

            await _cache.SetAsync($"refresh_token:{token.Token}", refreshTokenToBytes);
        }

        private async Task<RefreshToken?> GetRefreshTokenFromCacheAsync(string token)
        {
            var refreshTokenBytes = await _cache.GetAsync($"refresh_token:{token}");

            if (refreshTokenBytes == null)
                return null;

            return JsonSerializer.Deserialize<RefreshToken>(refreshTokenBytes);
        }

        private Task RemoveRefreshTokenFromCacheAsync(string token)
        {
            return _cache.RemoveAsync($"refresh_token:{token}");
        }


        #endregion Private Methods
    }
}
