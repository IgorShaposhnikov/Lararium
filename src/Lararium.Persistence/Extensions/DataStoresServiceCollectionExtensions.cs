using Lararium.Persistence.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Lararium.Persistence.Extensions
{
    public static class DataStoresServiceCollectionExtensions
    {
        extension(IServiceCollection services)
        {
            public IServiceCollection AddDataStores()
            {
                var assembly = typeof(AppDbContext).Assembly;

                var types = assembly.GetTypes()
                    .Where(t => t.IsClass && !t.IsAbstract)
                    .Select(t => new
                    {
                        Type = t,
                        Interfaces = t.GetInterfaces()
                            .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDataStore<,>))
                            .ToList()
                    })
                    .Where(x => x.Interfaces.Count != 0);

                foreach (var typeInfo in types)
                {
                    foreach (var iface in typeInfo.Interfaces)
                    {
                        services.AddScoped(iface, typeInfo.Type);
                    }
                }

                return services;
            }
        }
    }
}
