using Lararium.Core.AspNetCore;
using Lararium.Core.Modules;
using Lararium.Video.Encoders;
using Lararium.Video.Models.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lararium.Video
{
    public sealed class ModuleInitializer : IAspNetCoreModuleInitializer
    {
        private readonly ModuleMetadata _metadata = new(
            id: Guid.Parse("ad74efa7-8a65-4592-b03d-6b175114d7e7"),
            name: "Lararium Video",
            assembly: typeof(ModuleInitializer).Assembly,
            version: "1.0.0",
            priority: 10,
            hasApiControllers: true
        );

        public ModuleMetadata GetMetadata()
        {
            return _metadata;
        }

        public IServiceCollection AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<VideoServiceOptions>()
                .Bind(configuration.GetSection("VideoService"));

            services.AddScoped<IVideoEncoder<HlsEncoder>, HlsEncoder>();

            return services;
        }

        public void ConfigureEndpoints(IEndpointRouteBuilder endpoints)
        {

        }

        public void ConfigureMiddleware(IApplicationBuilder app)
        {

        }
    }
}
