using Lararium.Authorization.Jwt.Exceptions;
using Lararium.Authorization.Jwt.Models;
using Lararium.Authorization.Jwt.Models.Requests;
using Lararium.Authorization.Jwt.Models.Response;
using Lararium.Core;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text.Json;

namespace Lararium.Authorization.Jwt.Services;

internal sealed class JwtIdentityService(IUserDataStore userDataStore, IDistributedCache cache, JwtTokenService tokenService) : IJwtIdentityService
{
    public async Task<AuthenticationResponse> LoginUserAsync(LoginRequest request, CancellationToken cancellationToken = default!)
    {
        var user = await userDataStore.GetAsync(u => u.Login == request.Login, cancellationToken: cancellationToken);

        UserNotFoundException.ThrowIfNull(user, request.Login);

        if (!tokenService.IsPasswordCorrect(request.Password, user!.PasswordHash!, user))
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        var claims = new List<Claim> {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Login)
        };

        var accessToken = tokenService.GenerateAccessToken(claims);
        var refreshToken = tokenService.GenerateRefreshToken();

        await SaveRefreshTokenToCacheAsync(refreshToken);

        return new AuthenticationResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        };
    }

    public async Task<AuthenticationResponse> RegisterUserAsync(RegisterRequest data, CancellationToken cancellationToken = default!)
    {
        var newUser = new LarariumUser()
        {
            Login = data.Login,
            FirstName = data.FirstName,
            LastName = data.LastName,
            PasswordHash = tokenService.GenerateHashFromString(data.Password)
        };

        await userDataStore.AddAsync(newUser, cancellationToken);
        await userDataStore.SaveChangesAsync(cancellationToken: cancellationToken);

        return await LoginUserAsync(new LoginRequest { Login = data.Login, Password = data.Password }, cancellationToken);
    }

    public Task<bool> IsUserExistsAsync(string login, CancellationToken cancellationToken = default)
    {
        return userDataStore.IsExists(x => x.Login == login, cancellationToken);
    }


    public async Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        var principles = await tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
        var userId = principles.FindFirstValue(ClaimTypes.NameIdentifier);
        var userName = principles.FindFirstValue(ClaimTypes.Name);

        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userName))
        {
            throw new SecurityTokenException("Invalid AccessToken claims");
        }

        var storedRefreshToken = await GetRefreshTokenFromCacheAsync(request.RefreshToken);
        var isRefreshTokenValid = storedRefreshToken != null && DateTime.UtcNow <= storedRefreshToken!.Expires;
        if (!isRefreshTokenValid)
        {
            throw new SecurityTokenException("Invalid RefreshToken");
        }

        var newRefreshToken = tokenService.GenerateRefreshToken();

        List<Claim> claims = [
            new(ClaimTypes.NameIdentifier, userId),
            new(ClaimTypes.Name, userName)
        ];

        var newAccessToken = tokenService.GenerateAccessToken(claims);

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

        await cache.SetAsync($"refresh_token:{token.Token}", refreshTokenToBytes);
    }

    private async Task<RefreshToken?> GetRefreshTokenFromCacheAsync(string token)
    {
        var refreshTokenBytes = await cache.GetAsync($"refresh_token:{token}");

        if (refreshTokenBytes == null)
            return null;

        return JsonSerializer.Deserialize<RefreshToken>(refreshTokenBytes);
    }

    private Task RemoveRefreshTokenFromCacheAsync(string token)
    {
        return cache.RemoveAsync($"refresh_token:{token}");
    }


    #endregion Private Methods
}
