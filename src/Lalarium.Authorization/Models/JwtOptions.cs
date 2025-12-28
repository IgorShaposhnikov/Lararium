namespace Lararium.Authorization.Jwt.Models
{
    public class JwtOptions
    {
        public string Key { get; set; }
        public JwtHashConfig HashConfig { get; set; }
        public TimeSpan AccessTokenExpiration { get; set; }
        public TimeSpan RefreshTokenExpiration { get; set; }
    }

    public class JwtHashConfig 
    {
        public string Salt { get; set; }
        public int Iteration { get; set; }
        public int Length { get; set; }
    }
}
