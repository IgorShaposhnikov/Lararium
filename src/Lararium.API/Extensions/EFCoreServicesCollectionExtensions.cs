using Lararium.Persistence;
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

                return services;
            }
        }
    }
}
