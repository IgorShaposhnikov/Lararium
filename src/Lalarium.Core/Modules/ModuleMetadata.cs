using System.Reflection;

namespace Lararium.Core.Modules
{
    public struct ModuleMetadata
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Version { get; init; }
        public Assembly Assembly { get; init; }
        public bool HasApiControllers { get; init; }
    }
}
