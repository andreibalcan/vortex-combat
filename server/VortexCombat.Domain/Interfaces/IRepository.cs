using System.Linq.Expressions;
using VortexCombat.Domain.Specifications;

namespace VortexCombat.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        // Specification support
        Task<List<T>> ListAsync(ISpecification<T> spec);
        Task<T?> FirstOrDefaultAsync(ISpecification<T> spec);

        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Update(T entity);
        void Remove(T entity);
        Task<int> SaveChangesAsync();
    }
}