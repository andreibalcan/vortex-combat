using Microsoft.EntityFrameworkCore;
using VortexCombat.Domain.Entities;
using VortexCombat.Domain.Interfaces;
using VortexCombat.Infrastructure.Data;

namespace VortexCombat.Infrastructure.Repositories
{
    public class ExerciseRepository : Repository<Exercise>, IExerciseRepository
    {
        public ExerciseRepository(ApplicationDbContext context) : base(context) { }

        public Task<List<Exercise>> GetByBeltAsync(Belt belt)
        {
            return _dbSet.Where(e => e.Grade.Color == belt.Color && e.Grade.Degrees == belt.Degrees)
                .ToListAsync();
        }
    }
}