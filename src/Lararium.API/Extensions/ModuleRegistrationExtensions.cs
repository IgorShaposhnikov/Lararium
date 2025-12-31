using Lararium.Core.Modules;

namespace Lararium.API.Extensions
{
    public static class ModuleRegistrationExtensions
    {
        private static ICollection<IModuleInitializer> _modules = [];

        extension(IMvcBuilder mvcBuilder)
        {
            public IMvcBuilder AddModuleControllers() 
            {
                var modules = GetModules();

                foreach (var module in modules) 
                {
                    var metadata = module.GetMetadata();
                    if (metadata.HasApiControllers) 
                    {
                        mvcBuilder.AddApplicationPart(metadata.Assembly);
                    }
                }

                return mvcBuilder;
            }
        }

        extension(IServiceCollection services) 
        {
            public IServiceCollection AddModuleServices(IConfiguration configuration, params HashSet<string> excludeModules)
            {
                var modules = GetModules();

                if (excludeModules != null || excludeModules.Count > 0)
                {
                    modules = modules
                        .Where(module => 
                            !excludeModules.Contains(module.GetMetadata().Id.ToString()) &&
                            !excludeModules.Contains(module.GetMetadata().Name));
                }


                foreach (var module in modules)
                {
                    module.AddServices(services, configuration);
                }

                return services;
            }
        }

        public static IEnumerable<IModuleInitializer> GetModules()
        {
            if (_modules.Count > 0)
            {
                return _modules;
            }

            var initializerInterfaceType = typeof(IModuleInitializer);

            var initializerTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => initializerInterfaceType.IsAssignableFrom(type) &&
                               !type.IsInterface &&
                               !type.IsAbstract);

            foreach (var type in initializerTypes)
            {
                // 1. Создаем экземпляр класса (вызывается конструктор без параметров)
                var instance = (IModuleInitializer)Activator.CreateInstance(type);
                _modules.Add(instance);
            }

            return _modules;
        }
    }
}
