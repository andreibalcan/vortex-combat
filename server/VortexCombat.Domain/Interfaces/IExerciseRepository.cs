using VortexCombat.Domain.Entities;

namespace VortexCombat.Domain.Interfaces
{
    public interface IExerciseRepository : IRepository<Exercise>
    {
        Task<List<Exercise>> GetByBeltAsync(Belt belt);
    }
}