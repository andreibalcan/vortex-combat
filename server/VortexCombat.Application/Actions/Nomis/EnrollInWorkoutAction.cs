using VortexCombat.Application.Specifications;
using VortexCombat.Domain.Interfaces;
using VortexCombat.Shared.Enums;

namespace VortexCombat.Application.Actions.Nomis
{
    public sealed class EnrollInWorkoutAction : INomisAction<EnrollWorkoutRequest, (int studentId, string studentName)>
    {
        private readonly IWorkoutRepository _workoutRepo;
        private readonly IStudentRepository _studentRepo;

        public EnrollInWorkoutAction(IWorkoutRepository workoutRepo, IStudentRepository studentRepo)
        {
            _workoutRepo = workoutRepo;
            _studentRepo = studentRepo;
        }

        public async Task<(bool ok, string? error)> CanExecuteAsync(EnrollWorkoutRequest req, CancellationToken ct = default)
        {
            var workout = await _workoutRepo.FirstOrDefaultAsync(new WorkoutByIdSpec(req.WorkoutId));
            if (workout is null) return (false, "Workout not found");

            if (await _workoutRepo.IsStudentEnrolledAsync(req.WorkoutId, req.StudentId))
                return (false, "You are already enrolled in this workout.");

            if (await _workoutRepo.StudentHasTimeConflictAsync(req.StudentId, workout.StartDate, workout.EndDate))
                return (false, "You are already enrolled in another workout during this time.");

            return (true, null);
        }

        public async Task<(int studentId, string studentName)> ExecuteAsync(EnrollWorkoutRequest req, CancellationToken ct = default)
        {
            await _workoutRepo.EnrollStudentAsync(req.WorkoutId, req.StudentId, EAttendanceStatus.Enrolled);
            var student = await _studentRepo.GetByIdWithUserAsync(req.StudentId);
            return (student!.Id, student.ApplicationUser.Name);
        }
    }
}