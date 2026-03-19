using Lararium.Authorization.Jwt.Models.Requests;
using Lararium.Authorization.Jwt.Models.Response;

namespace Lararium.Authorization.Jwt
{
    public interface IJwtIdentityService
    {
        Task<AuthenticationResponse> RegisterUserAsync(RegisterRequest request, CancellationToken cancellationToken = default);
        Task<AuthenticationResponse> LoginUserAsync(LoginRequest request, CancellationToken cancellationToken = default);
        Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default);
    }
}
