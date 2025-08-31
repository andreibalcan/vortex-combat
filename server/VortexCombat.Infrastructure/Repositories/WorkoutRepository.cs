using Microsoft.EntityFrameworkCore;
using VortexCombat.Domain.Entities;
using VortexCombat.Domain.Interfaces;
using VortexCombat.Infrastructure.Data;
using VortexCombat.Shared.Enums;

namespace VortexCombat.Infrastructure.Repositories
{
    public class WorkoutRepository : Repository<Workout>, IWorkoutRepository
    {
        public WorkoutRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Task<List<Workout>> GetAllWithDetailsAsync()
        {
            return _dbSet
                .Include(w => w.WorkoutStudents).ThenInclude(ws => ws.Student).ThenInclude(s => s.ApplicationUser)
                .Include(w => w.WorkoutMasters).ThenInclude(wm => wm.Master).ThenInclude(m => m.ApplicationUser)
                .Include(w => w.WorkoutExercises).ThenInclude(we => we.Exercise)
                .ToListAsync();
        }

        public Task<bool> IsStudentEnrolledAsync(int workoutId, int studentId)
        {
            return _context.WorkoutStudents
                .AnyAsync(ws => ws.WorkoutId == workoutId && ws.StudentId == studentId);
        }

        public Task<bool> StudentHasTimeConflictAsync(int studentId, DateTime start, DateTime end)
        {
            return _context.WorkoutStudents
                .Where(ws => ws.StudentId == studentId)
                .AnyAsync(ws =>
                    _context.Workouts.Any(w =>
                        w.Id == ws.WorkoutId &&
                        w.StartDate < end &&
                        start < w.EndDate));
        }

        public async Task EnrollStudentAsync(int workoutId, int studentId, EAttendanceStatus status)
        {
            _context.WorkoutStudents.Add(new WorkoutStudent
            {
                WorkoutId = workoutId,
                StudentId = studentId,
                Status = status
            });
            await _context.SaveChangesAsync();
        }

        public async Task MarkAttendanceAsync(int workoutId, IEnumerable<int> studentIds, IEnumerable<int> masterIds)
        {
            var students = await _context.Students.Where(s => studentIds.Contains(s.Id)).ToListAsync();
            if (students.Count != studentIds.Count()) throw new InvalidOperationException("Some students not found");

            var masters = await _context.Masters.Where(m => masterIds.Contains(m.Id)).ToListAsync();
            if (masters.Count != masterIds.Count()) throw new InvalidOperationException("Some masters not found");

            foreach (var s in students)
            {
                var ws = await _context.WorkoutStudents
                    .FirstOrDefaultAsync(x => x.WorkoutId == workoutId && x.StudentId == s.Id);
                if (ws == null)
                    _context.WorkoutStudents.Add(new WorkoutStudent
                        { WorkoutId = workoutId, StudentId = s.Id, Status = EAttendanceStatus.Attended });
                else
                    ws.Status = EAttendanceStatus.Attended;
            }

            foreach (var m in masters)
            {
                var wm = await _context.WorkoutMasters
                    .FirstOrDefaultAsync(x => x.WorkoutId == workoutId && x.MasterId == m.Id);
                if (wm == null)
                    _context.WorkoutMasters.Add(new WorkoutMaster
                        { WorkoutId = workoutId, MasterId = m.Id, Status = EAttendanceStatus.Attended });
                else
                    wm.Status = EAttendanceStatus.Attended;
            }

            await _context.SaveChangesAsync();

            var workoutExercises = await GetWorkoutExercisesAsync(workoutId);
            foreach (var s in students)
            {
                foreach (var ex in workoutExercises)
                {
                    var already = await _context.StudentWorkoutExercise.AnyAsync(swe =>
                        swe.WorkoutId == workoutId && swe.StudentId == s.Id && swe.ExerciseId == ex.ExerciseId);

                    if (!already)
                        _context.StudentWorkoutExercise.Add(new StudentWorkoutExercise
                        {
                            WorkoutId = workoutId,
                            StudentId = s.Id,
                            ExerciseId = ex.ExerciseId
                        });
                }
            }

            await _context.SaveChangesAsync();
        }

        public Task<List<WorkoutExercise>> GetWorkoutExercisesAsync(int workoutId)
        {
            return _context.WorkoutExercise.Where(we => we.WorkoutId == workoutId).ToListAsync();
        }

        public async Task<List<int>> GetWorkoutExerciseIdsAsync(int workoutId)
        {
            return await _context.WorkoutExercise
                .Where(we => we.WorkoutId == workoutId)
                .Select(we => we.ExerciseId)
                .ToListAsync();
        }

        public async Task UpdateWorkoutExercisesAsync(int workoutId, List<int> exerciseIds)
        {
            var existingExerciseIds = await GetWorkoutExerciseIdsAsync(workoutId);

            var exercisesToRemove = existingExerciseIds.Except(exerciseIds);
            if (exercisesToRemove.Any())
            {
                var workoutExercisesToRemove = await _context.WorkoutExercise
                    .Where(we => we.WorkoutId == workoutId && exercisesToRemove.Contains(we.ExerciseId))
                    .ToListAsync();
                _context.WorkoutExercise.RemoveRange(workoutExercisesToRemove);
            }

            var exercisesToAdd = exerciseIds.Except(existingExerciseIds);
            foreach (var exerciseId in exercisesToAdd)
            {
                _context.WorkoutExercise.Add(new WorkoutExercise
                {
                    WorkoutId = workoutId,
                    ExerciseId = exerciseId
                });
            }

            await _context.SaveChangesAsync();
        }

        public Task<Workout?> GetByIdWithDetailsAsync(int id)
        {
            return _dbSet
                .AsNoTracking()
                .Include(w => w.WorkoutStudents).ThenInclude(ws => ws.Student).ThenInclude(s => s.ApplicationUser)
                .Include(w => w.WorkoutMasters).ThenInclude(wm => wm.Master).ThenInclude(m => m.ApplicationUser)
                .Include(w => w.WorkoutExercises).ThenInclude(we => we.Exercise)
                .FirstOrDefaultAsync(w => w.Id == id);
        }
    }
}