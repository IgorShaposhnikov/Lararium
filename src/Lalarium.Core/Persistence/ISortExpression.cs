using System.Linq.Expressions;

namespace Lararium.Core.Persistence
{
    /// <summary>
    /// Represents a sort expression for data queries.
    /// Allows defining the sort field and direction (ascending/descending).
    /// </summary>
    /// <typeparam name="T">The type of entity the sorting is applied to.</typeparam>
    /// <remarks>
    /// This class is used to pass sorting parameters to repository methods,
    /// providing flexibility and type safety when building sorted queries.
    /// 
    /// Usage example:
    /// <code>
    /// var sortByDate = new SortExpression&lt;VideoEntity&gt;
    /// {
    ///     KeySelector = v => v.CreatedAt,
    ///     Descending = true
    /// };
    /// 
    /// var sortByName = new SortExpression&lt;VideoEntity&gt;
    /// {
    ///     KeySelector = v => v.Title,
    ///     Descending = false
    /// };
    /// 
    /// // Passing to a repository method
    /// var videos = await repository.GetEntitiesAsync(
    ///     orderBy: new List&lt;SortExpression&lt;VideoEntity&gt;&gt; { sortByDate, sortByName }
    /// );
    /// </code>
    /// 
    /// In SQL, this translates to: ORDER BY CreatedAt DESC, Title ASC
    /// </remarks>
    public interface ISortExpression<T>
    {
        /// <summary>
        /// Gets or sets the expression used to select the sort key.
        /// Determines the field by which sorting will be performed.
        /// </summary>
        /// <value>
        /// A lambda expression that selects the entity property for sorting.
        /// For example: to sort by creation date: <c>v => v.CreatedAt</c>
        /// </value>
        Expression<Func<T, object>> KeySelector { get; init; }

        /// <summary>
        /// Gets or sets a value indicating the sort direction.
        /// </summary>
        /// <value>
        /// <c>true</c> for descending order (DESC); <c>false</c> for ascending order (ASC).
        /// Default value: <c>false</c> (ascending).
        /// </value>
        bool Descending { get; init; }
    }

}
