using Microsoft.EntityFrameworkCore;
using VortexCombat.Domain.Entities;
using VortexCombat.Domain.Interfaces;
using VortexCombat.Infrastructure.Data;
using VortexCombat.Shared.Enums;

namespace VortexCombat.Infrastructure.Repositories
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        public StudentRepository(ApplicationDbContext context) : base(context) { }

        public Task<List<Student>> GetAllWithUserAsync()
            => _dbSet.Include(s => s.ApplicationUser).ToListAsync();

        public Task<Student?> GetByIdWithUserAsync(int id)
            => _dbSet.Include(s => s.ApplicationUser).FirstOrDefaultAsync(s => s.Id == id);

        public Task<Student?> GetByApplicationUserIdAsync(string userId)
            => _dbSet.Include(s => s.ApplicationUser).FirstOrDefaultAsync(s => s.ApplicationUserId == userId);

        public Task<List<Workout>> GetAttendedWorkoutsAsync(int studentId)
            => _context.WorkoutStudents
                .Where(ws => ws.StudentId == studentId && ws.Status == EAttendanceStatus.Attended)
                .Select(ws => ws.Workout)
                .ToListAsync();

        public Task<List<Exercise>> GetCompletedExercisesForBeltAsync(int studentId, Belt belt)
        {
            var requiredInBelt = _context.Exercises
                .Where(e => e.Grade.Color == belt.Color && e.Grade.Degrees == belt.Degrees)
                .Select(e => e.Id);

            return _context.StudentWorkoutExercise
                .Where(swe => swe.StudentId == studentId && requiredInBelt.Contains(swe.ExerciseId))
                .Select(swe => swe.Exercise)
                .Distinct()
                .ToListAsync();
        }
    }
}