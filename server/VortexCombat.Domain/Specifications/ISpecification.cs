using System.Linq.Expressions;

namespace VortexCombat.Domain.Specifications
{
    public interface ISpecification<T>
    {
        bool IsSatisfiedBy(T entity);
    }
}