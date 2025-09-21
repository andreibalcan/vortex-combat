
using Microsoft.EntityFrameworkCore;
using VortexCombat.Domain.Entities;
using VortexCombat.Domain.Interfaces;
using VortexCombat.Infrastructure.Data;

namespace VortexCombat.Infrastructure.Repositories
{
    public class MasterRepository : Repository<Master>, IMasterRepository
    {
        public MasterRepository(ApplicationDbContext context) : base(context) { }

        public Task<List<Master>> GetAllWithUserAsync()
        {
            return _dbSet.Include(m => m.User).ToListAsync();
        }
    }
}