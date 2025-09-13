using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VortexCombat.Domain.Interfaces;
using VortexCombat.Domain.Specifications;
using VortexCombat.Infrastructure.Data;

namespace VortexCombat.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
            => await _dbSet.Where(predicate).ToListAsync();

        // List using specification
        public async Task<List<T>> ListAsync(ISpecification<T>? spec = null)
        {
            if (spec == null) 
                return await _dbSet.ToListAsync();

            // Fetch all and filter in memory
            var allItems = await _dbSet.ToListAsync();
            return allItems.Where(spec.IsSatisfiedBy).ToList();
        }

        // FirstOrDefault using specification
        public async Task<T?> FirstOrDefaultAsync(ISpecification<T>? spec = null)
        {
            if (spec == null) 
                return await _dbSet.FirstOrDefaultAsync();

            var allItems = await _dbSet.ToListAsync();
            return allItems.FirstOrDefault(spec.IsSatisfiedBy);
        }

        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public async Task AddRangeAsync(IEnumerable<T> entities) => await _dbSet.AddRangeAsync(entities);

        public void Update(T entity) => _dbSet.Update(entity);

        public void Remove(T entity) => _dbSet.Remove(entity);

        public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
