namespace Lararium.API.Extensions
{
    public static class OptionsServicesCollectionExtensions
    {
        extension(IServiceCollection services)
        {
            public void RegisterOptions(ConfigurationManager configuration)
            {
                services.AddOptions<AuthorizationOptions>()
                    .Bind(configuration.GetSection("Jwt"))
                    .ValidateOnStart();
            }
        }
    }
}
