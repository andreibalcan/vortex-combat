using VortexCombat.Domain.Entities;

namespace VortexCombat.Domain.Interfaces
{
    public interface IMasterRepository : IRepository<Master>
    {
        Task<List<Master>> GetAllWithUserAsync();
    }
}