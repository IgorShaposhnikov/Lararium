using Lararium.Authorization.Jwt.Models.Requests;
using Lararium.Authorization.Jwt.Models.Response;

namespace Lararium.Authorization.Jwt
{
    public interface IJwtAuthorizationProvider
    {
        Task<AuthenticationResponse> RegisterUserAsync(RegisterRequest data, CancellationToken cancellationToken = default);
        Task<AuthenticationResponse> LoginUserAsync(string login, string password, CancellationToken cancellationToken = default);
        Task<bool> IsUserExistsAsync(string login,CancellationToken cancellationToken = default);
        Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default);
    }
}
