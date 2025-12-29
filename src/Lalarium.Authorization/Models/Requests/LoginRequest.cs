namespace Lararium.Authorization.Jwt.Models.Requests
{
    public class LoginRequest
    {
        public string Login { get; init; }
        public string Password { get; init; }
    }
}
