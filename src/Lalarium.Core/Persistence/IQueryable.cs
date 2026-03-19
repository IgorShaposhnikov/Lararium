namespace Lararium.Core.Persistence
{
    /// <summary>
    /// Represents a delegate for defining entity inclusion (eager loading) strategies in queries.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity being queried.</typeparam>
    /// <param name="query">The base query to apply inclusion operations to.</param>
    /// <returns>
    /// An <see cref="IQueryable{TEntity}"/> with the specified related entities eagerly loaded.
    /// </returns>
    /// <remarks>
    /// This delegate is used to encapsulate Entity Framework Core's Include/ThenInclude patterns
    /// in a reusable and composable manner. It enables separation of query composition from
    /// business logic and supports advanced eager loading scenarios.
    /// 
    /// Example usage:
    /// <code>
    /// IncludeQuery&lt;VideoEntity&gt; include = query => query
    ///     .Include(v => v.Tags)
    ///     .ThenInclude(t => t.Category)
    ///     .Include(v => v.Actors)
    ///     .ThenInclude(a => a.Metadata);
    /// </code>
    public delegate IQueryable<TEntity> IncludeQuery<TEntity>(IQueryable<TEntity> query);
}
