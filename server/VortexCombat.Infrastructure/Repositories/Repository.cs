using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VortexCombat.Domain.Interfaces;
using VortexCombat.Domain.Specifications;
using VortexCombat.Infrastructure.Data;
using VortexCombat.Infrastructure.Specifications;

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

        public async Task<List<T>> ListAsync(ISpecification<T> spec)
            => await SpecificationEvaluator.GetQuery(_dbSet.AsQueryable(), spec).ToListAsync();

        public async Task<T?> FirstOrDefaultAsync(ISpecification<T> spec)
            => await SpecificationEvaluator.GetQuery(_dbSet.AsQueryable(), spec).FirstOrDefaultAsync();

        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
        public async Task AddRangeAsync(IEnumerable<T> entities) => await _dbSet.AddRangeAsync(entities);
        public void Update(T entity) => _dbSet.Update(entity);
        public void Remove(T entity) => _dbSet.Remove(entity);
        public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
    }
}