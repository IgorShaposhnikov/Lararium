using Lararium.Persistence;
using Lararium.Persistence.Extensions;

namespace Lararium.API.Extensions
{
    internal static class DatabaseServicesCollectionExtensions
    {
        extension(WebApplicationBuilder builder)
        {
            internal WebApplicationBuilder AddDbContext(string connectionString)
            {
                builder.AddNpgsqlDbContext<AppDbContext>(connectionName: connectionString);

                builder.Services.AddDataStores();

                return builder;
            }
        }
    }
}
