using Lararium.Core.Modules;
using Lararium.Video.Models.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lararium.Video
{
    public sealed class ModuleInitializer : IModuleInitializer
    {
        private readonly ModuleMetadata _metadata = new()
        {
            Id = Guid.Parse("ad74efa7-8a65-4592-b03d-6b175114d7e7"),
            Name = "Lararium Video",
            Assembly = typeof(ModuleInitializer).Assembly,
            Version = "1.0.0",
            HasApiControllers = true,
        };

        public IServiceCollection AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<VideoServiceOptions>()
                .Bind(configuration.GetSection("VideoService"));

            return services;
        }

        public ModuleMetadata GetMetadata()
        {
            return _metadata;
        }
    }
}
