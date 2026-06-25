namespace Lararium.Authorization.Jwt.Models;

public sealed class RefreshToken
{
    public string Token { get; set; }
    public DateTime Expires { get; set; }
    public DateTime Created { get; set; }
}
