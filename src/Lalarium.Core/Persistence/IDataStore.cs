using System.Linq.Expressions;

namespace Lararium.Core.Persistence
{
    /// <summary>
    /// Generic repository interface defining data access operations for entities.
    /// Provides a contract for CRUD operations and advanced querying capabilities.
    /// </summary>
    /// <typeparam name="TEntity">The entity type managed by this repository.</typeparam>
    /// <typeparam name="TId">The type of the entity's primary key identifier.</typeparam>
    public interface IDataStore<TEntity, TId> where TEntity : class
    {
        /// <summary>
        /// Retrieves a single entity by its unique identifier.
        /// </summary>
        /// <param name="id">The primary key value of the entity to retrieve.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains
        /// the found entity or null if no entity with the specified id exists.
        /// </returns>
        Task<TEntity?> GetAsync(TId id, bool asTracking = true, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a single entity using a custom filter expression with eager loading.
        /// </summary>
        /// <param name="filter">A predicate expression to filter the entity.</param>
        /// <param name="includeQuery">A query for eager loading related entities.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains
        /// the first entity matching the filter or null if no match is found.
        /// </returns>
        /// <remarks>
        /// Use this overload for complex queries that require filtering and including related entities.
        /// </remarks>
        Task<TEntity?> GetAsync(
            Expression<Func<TEntity, bool>> filter,
            IncludeQuery<TEntity>? includeQuery = default,
            bool asTracking = true,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a new entity to the data store.
        /// </summary>
        /// <param name="entity">The entity to add to the data store.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous add operation.</returns>
        /// <remarks>
        /// The entity is not persisted to the database until <see cref="SaveChangesAsync"/> is called.
        /// </remarks>
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Marks an existing entity as modified for update.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <remarks>
        /// This method sets the entity state to Modified. The changes are not persisted
        /// to the database until <see cref="SaveChangesAsync"/> is called.
        /// </remarks>
        void Update(TEntity entity);

        /// <summary>
        /// Checks if an entity with the specified identifier exists in the data store.
        /// </summary>
        /// <param name="id">The primary key value to check for existence.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains
        /// true if an entity with the specified id exists; otherwise, false.
        /// </returns>
        Task<bool> IsExists(TId id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if any entity matches the specified predicate.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains
        /// true if any elements satisfy the predicate; otherwise, false.
        /// </returns>
        Task<bool> IsExists(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all entities from the data store.
        /// </summary>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains
        /// a collection of all entities in the data store.
        /// </returns>
        /// <remarks>
        /// Consider using paginated queries for large datasets via <see cref="GetEntitiesAsync"/>.
        /// </remarks>
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a collection of entities with optional filtering, sorting, and pagination.
        /// </summary>
        /// <param name="where">Optional filter expression to apply to the query.</param>
        /// <param name="orderBy">Optional list of sort expressions to order the results.</param>
        /// <param name="includeQuery">Optional query for eager loading related entities.</param>
        /// <param name="skip">Optional number of entities to skip (for pagination).</param>
        /// <param name="take">Optional maximum number of entities to return (for pagination).</param>
        /// <param name="asNoTracking">
        /// When true, entities are returned without change tracking. This improves performance
        /// for read-only scenarios.
        /// </param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains
        /// a collection of entities matching the specified criteria.
        /// </returns>
        Task<IEnumerable<TEntity>> GetEntitiesAsync(
            Expression<Func<TEntity, bool>>? where = null,
            List<SortExpression<TEntity>>? orderBy = null,
            IncludeQuery<TEntity>? includeQuery = null,
            int? skip = null,
            int? take = null,
            bool asNoTracking = true,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a collection of projected results with optional filtering, sorting, and pagination.
        /// </summary>
        /// <typeparam name="TResult">The type of the projected result.</typeparam>
        /// <param name="select">Projection expression to transform entities to the result type.</param>
        /// <param name="where">Optional filter expression to apply to the query.</param>
        /// <param name="orderBy">Optional list of sort expressions to order the results.</param>
        /// <param name="includeQuery">Optional query for eager loading related entities.</param>
        /// <param name="skip">Optional number of entities to skip (for pagination).</param>
        /// <param name="take">Optional maximum number of entities to return (for pagination).</param>
        /// <param name="asNoTracking">
        /// When true, entities are queried without change tracking. This improves performance
        /// for read-only scenarios.
        /// </param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains
        /// a collection of projected results matching the specified criteria.
        /// </returns>
        /// <remarks>
        /// This overload is efficient for read-only scenarios as it projects directly
        /// in the database query, reducing data transfer and memory usage.
        /// If I understand the EF Core correctly. We don't need flag asNoTracking here because we have a Select expression.
        /// </remarks>
        Task<IEnumerable<TResult>> GetEntitiesAsync<TResult>(
            Expression<Func<TEntity, TResult>> select,
            Expression<Func<TEntity, bool>>? where = null,
            List<SortExpression<TEntity>>? orderBy = null,
            IncludeQuery<TEntity>? includeQuery = null,
            int? skip = null,
            int? take = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Persists all changes made in this context to the underlying database.
        /// </summary>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>
        /// A task that represents the asynchronous save operation. The task result contains
        /// the number of state entries written to the database.
        /// </returns>
        /// <remarks>
        /// This method should be called after adding, updating, or deleting entities
        /// to persist the changes to the database.
        /// </remarks>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
