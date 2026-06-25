namespace Lararium.Authorization.Jwt.Models.Requests;

public sealed class RefreshTokenRequest
{
    /// <summary>
    /// The expired or current access token.
    /// </summary>
    public string? AccessToken { get; set; }

    /// <summary>
    /// The refresh token used to obtain a new token pair.
    /// </summary>
    public string? RefreshToken { get; set; }
}
