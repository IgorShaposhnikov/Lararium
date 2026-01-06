using Lararium.Video;
using System.Linq.Expressions;

namespace Lararium.Persistence.Core
{
    public delegate IQueryable<TEntity> IncludeQuery<TEntity>(IQueryable<TEntity> query);

    public interface IDataStore<TEntity, TId> where TEntity : class
    {
        Task<TEntity?> GetAsync(TId id, CancellationToken cancellationToken = default);
        Task<TEntity?> GetAsync(
            Expression<Func<TEntity, bool>> filter,
            IncludeQuery<TEntity> includeQuery,
            CancellationToken cancellationToken = default);
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        void Update(TEntity entity, CancellationToken cancellationToken = default);
        Task<bool> IsExists(TId id, CancellationToken cancellationToken = default);
        Task<bool> IsExists(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<TResult>> GetEntitiesAsync<TResult>(
            Expression<Func<TEntity, bool>>? where = null,
            Expression<Func<TEntity, TResult>>? select = null,
            List<SortExpression<TEntity>>? orderBy = null,
            IncludeQuery<TEntity>? includeQuery = null,
            int? skip = null,
            int? take = null,
            bool asNoTracking = true,
            CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
