using Lararium.Core.Persistence;

namespace Lararium.Core
{
    public interface IUserDataStore : IDataStore<LarariumUser, Guid>
    {
    }
}
