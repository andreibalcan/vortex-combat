using VortexCombat.Domain.Common;
using VortexCombat.Domain.Entities;

namespace VortexCombat.Domain.Interfaces
{
    public interface IStudentRepository : IRepository<Student>
    {
        Task<List<Student>> GetAllWithUserAsync();
        Task<Student?> GetByIdWithUserAsync(int id);
        //Task<Student?> GetByApplicationUserIdAsync(string userId);
        Task<Student?> GetByUserIdAsync(UserId userId); // Novo       
        Task<List<Workout>> GetAttendedWorkoutsAsync(int studentId);
        Task<List<Exercise>> GetCompletedExercisesForBeltAsync(int studentId, Belt belt);
    }
}