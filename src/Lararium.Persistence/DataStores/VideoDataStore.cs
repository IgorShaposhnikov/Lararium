using Lararium.Video.Models;
using Microsoft.Extensions.Logging;

namespace Lararium.Persistence.DataStores
{
    internal class VideoDataStore : DataStoreBase<VideoEntity, Guid>
    {
        public VideoDataStore(AppDbContext dbContext, ILogger<VideoDataStore> logger) : base(dbContext, logger) { }

        public override async Task<VideoEntity?> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Videos.FindAsync(id, cancellationToken);
        }

        public override Task<bool> IsExists(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
