using Lararium.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Lararium.Persistence.DataStores
{
    internal class UserDataStore(AppDbContext dbContext, ILogger<UserDataStore> logger) : DataStoreBase<LarariumUser, Guid>(dbContext, logger), IUserDataStore
    {
        public override async Task<LarariumUser?> GetAsync(Guid id, bool asNoTracking = true, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users.FindAsync(id, cancellationToken);
        }

        public override async Task<bool> IsExists(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users.AnyAsync(user => user.Id == id, cancellationToken);
        }
    }
}
