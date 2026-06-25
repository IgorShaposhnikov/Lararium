namespace Lararium.Authorization.Jwt.Models.Requests;

public sealed class LoginRequest
{
    /// <summary>
    /// Login
    /// </summary>
    public string Login { get; init; }
    /// <summary>
    /// Password
    /// </summary>
    public string Password { get; init; }
}
