namespace Lararium.Authorization.Jwt.Models.Response
{
    public class AuthenticationResponse
    {
        public string AccessToken { get; init; }
        public RefreshToken RefreshToken { get; init; }
    }
}
