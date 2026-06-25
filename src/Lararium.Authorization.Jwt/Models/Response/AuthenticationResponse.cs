namespace Lararium.Authorization.Jwt.Models.Response;

public sealed class AuthenticationResponse
{
    public string AccessToken { get; init; }
    public RefreshToken RefreshToken { get; init; }
}
