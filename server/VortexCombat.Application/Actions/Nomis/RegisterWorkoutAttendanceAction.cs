using VortexCombat.Application.Specifications;
using VortexCombat.Domain.Interfaces;

namespace VortexCombat.Application.Actions.Nomis
{
    public sealed class RegisterWorkoutAttendanceAction : INomisAction<RegisterAttendanceRequest, bool>
    {
        private readonly IWorkoutRepository _workoutRepo;
        private readonly IStudentRepository _studentRepo;
        private readonly IMasterRepository _masterRepo;

        public RegisterWorkoutAttendanceAction(
            IWorkoutRepository workoutRepo,
            IStudentRepository studentRepo,
            IMasterRepository masterRepo)
        {
            _workoutRepo = workoutRepo;
            _studentRepo = studentRepo;
            _masterRepo = masterRepo;
        }

        public async Task<(bool ok, string? error)> CanExecuteAsync(RegisterAttendanceRequest req,
            CancellationToken ct = default)
        {
            var workout = await _workoutRepo.FirstOrDefaultAsync(new WorkoutByIdSpec(req.WorkoutId));
            if (workout is null) return (false, "Workout not found");

            foreach (var sid in req.StudentIds)
                if (await _studentRepo.FirstOrDefaultAsync(new StudentByIdSpec(sid)) is null)
                    return (false, $"Student {sid} not found");

            foreach (var mid in req.MasterIds)
                if (await _masterRepo.FirstOrDefaultAsync(new MasterByIdSpec(mid)) is null)
                    return (false, $"Master {mid} not found");

            return (true, null);
        }

        public async Task<bool> ExecuteAsync(RegisterAttendanceRequest req, CancellationToken ct = default)
        {
            await _workoutRepo.MarkAttendanceAsync(req.WorkoutId, req.StudentIds, req.MasterIds);
            return true;
        }
    }
}