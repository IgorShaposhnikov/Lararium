using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lararium.Core.Modules
{
    public interface IModuleInitializer
    {
        ModuleMetadata GetMetadata();
        IServiceCollection AddServices(IServiceCollection services, IConfiguration configuration); 
    }
}
