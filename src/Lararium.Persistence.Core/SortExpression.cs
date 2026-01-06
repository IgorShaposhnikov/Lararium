using System.Linq.Expressions;

namespace Lararium.Video
{
    public class SortExpression<T>
    {
        public Expression<Func<T, object>> KeySelector { get; init; }
        public bool Descending { get; init; } = false;
    }
}
