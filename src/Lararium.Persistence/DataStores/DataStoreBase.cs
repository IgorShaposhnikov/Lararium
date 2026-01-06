using Lararium.Persistence.Core;
using Lararium.Video;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Lararium.Persistence.DataStores
{
    internal abstract class DataStoreBase<TEntity, TId> : IDataStore<TEntity, TId> where TEntity : class
    {
        protected readonly AppDbContext _dbContext;
        protected readonly ILogger<VideoDataStore> _logger;

        protected DataStoreBase(AppDbContext dbContext, ILogger<VideoDataStore> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity);

            try
            {
                await _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EFCore AddAsync error for video with id: {0}", entity);
                throw;
            }
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<TEntity>().ToListAsync(cancellationToken);
        }

        public abstract Task<TEntity?> GetAsync(TId id, CancellationToken cancellationToken = default);

        public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter, IncludeQuery<TEntity> includeQuery, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            if (includeQuery != null)
                query = includeQuery(query);

            if (filter != null)
                query = query.Where(filter);

            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public virtual async Task<IEnumerable<TResult>> GetEntitiesAsync<TResult>(
            Expression<Func<TEntity, bool>>? filter = null,
            Expression<Func<TEntity, TResult>>? select = null,
            List<SortExpression<TEntity>>? orderBy = null,
            IncludeQuery<TEntity>? include = null,
            int? skip = null,
            int? take = null,
            bool asNoTracking = true,
            CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            if (asNoTracking)
                query = query.AsNoTracking();

            if (include is not null)
                query = include(query);

            if (filter is not null)
                query = query.Where(filter);

            if (orderBy is not null && orderBy.Count != 0)
            {
                //orderby
                var firstSort = orderBy.First();
                IOrderedQueryable<TEntity> orderedQuery = firstSort.Descending
                    ? query.OrderByDescending(firstSort.KeySelector)
                    : query.OrderBy(firstSort.KeySelector);

                // thenby
                foreach (var sort in orderBy.Skip(1))
                {
                    orderedQuery = sort.Descending
                        ? orderedQuery.ThenByDescending(sort.KeySelector)
                        : orderedQuery.ThenBy(sort.KeySelector);
                }

                query = orderedQuery;
            }

            if (skip.HasValue)
                query = query.Skip(skip.Value);

            if (take.HasValue)
                query = query.Take(take.Value);

            ArgumentNullException.ThrowIfNull(select);

            return await query
                .Select(select)
                .ToListAsync(cancellationToken);
        }

        public abstract Task<bool> IsExists(TId id, CancellationToken cancellationToken = default);

        public Task<bool> IsExists(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return _dbContext
                .Set<TEntity>()
                .AsNoTracking()
                .AnyAsync(predicate, cancellationToken: cancellationToken);
        }

        public virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _dbContext.SaveChangesAsync(cancellationToken);
        }

        public virtual void Update(TEntity entity, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity);

            _dbContext.Set<TEntity>().Update(entity);
        }
    }
}
