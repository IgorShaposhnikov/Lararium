namespace Lararium.Authorization.Jwt.Models;

public sealed class JwtOptions
{
    public string Key { get; set; } = string.Empty;
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public bool ValidateIssuer { get; set; } = false;
    public bool ValidateAudience { get; set; } = false;
    public bool ValidateLifetime { get; set; } = true;
    public int ClockSkewSeconds { get; set; } = 0;
    public TimeSpan AccessTokenExpiration { get; set; }
    public TimeSpan RefreshTokenExpiration { get; set; }
}

public sealed class JwtHashConfig
{
    public string Salt { get; set; }
    public int Iteration { get; set; }
    public int Length { get; set; }
}
