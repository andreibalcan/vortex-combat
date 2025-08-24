using VortexCombat.Domain.Entities;
using VortexCombat.Shared.Enums;

namespace VortexCombat.Domain.Interfaces
{
    public interface IWorkoutRepository : IRepository<Workout>
    {
        Task<List<Workout>> GetAllWithDetailsAsync();
        Task<bool> IsStudentEnrolledAsync(int workoutId, int studentId);
        Task<bool> StudentHasTimeConflictAsync(int studentId, DateTime start, DateTime end);
        Task EnrollStudentAsync(int workoutId, int studentId, EAttendanceStatus status);
        Task MarkAttendanceAsync(int workoutId, IEnumerable<int> studentIds, IEnumerable<int> masterIds);
        Task<List<WorkoutExercise>> GetWorkoutExercisesAsync(int workoutId);
    }
}