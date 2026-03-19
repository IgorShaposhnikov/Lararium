using Lararium.Core.Persistence;
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

                // Find all non-abstract classes that implement IDataStore<,>
                var dataStoreTypes = assembly.GetTypes()
                    .Where(t => t.IsClass && !t.IsAbstract)
                    .Select(t => new
                    {
                        Type = t,
                        AllInterfaces = t.GetInterfaces(),
                        DataStoreInterfaces = t.GetInterfaces()
                            .Where(i => i.IsGenericType &&
                                       i.GetGenericTypeDefinition() == typeof(IDataStore<,>))
                            .ToList(),
                        // specialized interfaces, that implements IDataStore<,>
                        SpecializedInterfaces = t.GetInterfaces()
                            .Where(i => i.GetInterfaces()
                                .Any(ii => ii.IsGenericType &&
                                           ii.GetGenericTypeDefinition() == typeof(IDataStore<,>)))
                            .ToList()
                    })
                    .Where(x => x.DataStoreInterfaces.Count > 0 ||
                               x.SpecializedInterfaces.Count > 0);

                foreach (var typeInfo in dataStoreTypes)
                {
                    // 1. Register specialized interfaces first (e.g., IUserDataStore)
                    // This allows injecting specific data stores types instead of generic ones
                    foreach (var specialized in typeInfo.SpecializedInterfaces)
                    {
                        services.AddScoped(specialized, typeInfo.Type);
                    }

                    // 2. Register base IDataStore<,> only if there is no specialized interface
                    foreach (var dataStoreInterface in typeInfo.DataStoreInterfaces)
                    {
                        // Check if this base interface is already "hidden" behind a specialized one
                        var alreadyRegistered = typeInfo.SpecializedInterfaces
                            .Any(si => si.GetInterfaces().Contains(dataStoreInterface));

                        if (!alreadyRegistered)
                        {
                            services.AddScoped(dataStoreInterface, typeInfo.Type);
                        }
                    }
                }

                return services;
            }
        }
    }
}
