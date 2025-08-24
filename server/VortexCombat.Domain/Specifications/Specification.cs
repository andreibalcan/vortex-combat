using System.Linq.Expressions;

namespace VortexCombat.Domain.Specifications
{
    public abstract class Specification<T> : ISpecification<T>
    {
        public abstract Expression<Func<T, bool>> Criteria { get; }

        protected static Expression<Func<T, bool>> And(
            Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> right)
        {
            var param = Expression.Parameter(typeof(T));
            var body = Expression.AndAlso(
                Expression.Invoke(left, param),
                Expression.Invoke(right, param));
            return Expression.Lambda<Func<T, bool>>(body, param);
        }

        protected static Expression<Func<T, bool>> Or(
            Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> right)
        {
            var param = Expression.Parameter(typeof(T));
            var body = Expression.OrElse(
                Expression.Invoke(left, param),
                Expression.Invoke(right, param));
            return Expression.Lambda<Func<T, bool>>(body, param);
        }

        protected static Expression<Func<T, bool>> Not(Expression<Func<T, bool>> expr)
        {
            var param = Expression.Parameter(typeof(T));
            var body = Expression.Not(Expression.Invoke(expr, param));
            return Expression.Lambda<Func<T, bool>>(body, param);
        }
    }
}