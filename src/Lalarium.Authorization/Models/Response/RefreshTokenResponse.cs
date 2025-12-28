namespace Lararium.Authorization.Jwt.Models.Response
{
    public class RefreshTokenResponse
    {
        public string AccessToken { get; init; }
        public RefreshToken RefreshToken { get; init; }
    }
}
