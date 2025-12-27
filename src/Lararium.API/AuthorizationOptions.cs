namespace Lararium.API
{
    public class AuthorizationOptions
    {
        public string Key { get; set; }
        public JwtHashConfig HashConfig { get; set; }
    }

    public class JwtHashConfig 
    {
        public string Salt { get; set; }
        public int Iteration { get; set; }
        public int Length { get; set; }
    }
}
