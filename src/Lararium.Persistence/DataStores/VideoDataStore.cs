using Lararium.Video.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Lararium.Persistence.DataStores
{
    internal class VideoDataStore(AppDbContext dbContext, ILogger<VideoDataStore> logger) : DataStoreBase<VideoEntity, Guid>(dbContext, logger)
    {
        public override async Task<VideoEntity?> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Videos.FindAsync(id, cancellationToken);
        }

        public override Task<bool> IsExists(Guid id, CancellationToken cancellationToken = default)
        {
            return _dbContext.Videos.AnyAsync(e => e.Id == id, cancellationToken);
        }
    }
}
