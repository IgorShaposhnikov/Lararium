using Lararium.Persistence;
using Lararium.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Lararium.API.Extensions
{
    internal static class EFCoreServicesCollectionExtensions
    {
        extension(IServiceCollection services)
        {
            internal IServiceCollection AddDbContext(string connectionString)
            {
                services.AddDbContext<AppDbContext>(options =>
                    options.UseNpgsql(connectionString)
                    );

                services.AddDataStores();

                return services;
            }
        }
    }
}
