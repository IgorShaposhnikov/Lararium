using Lararium.API.Options;

namespace Lararium.API.Extensions
{
    internal static class OptionsServicesCollectionExtensions
    {
        extension(IServiceCollection services)
        {
            internal void RegisterOptions(ConfigurationManager configuration)
            {
                services.AddOptions<RedisOptions>()
                    .Bind(configuration.GetSection("Redis"));
            }
        }
    }
}
