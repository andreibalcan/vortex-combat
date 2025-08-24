using VortexCombat.Application.Specifications;
using VortexCombat.Domain.Interfaces;

namespace VortexCombat.Application.Actions.Nomis
{
    public sealed class DeleteWorkoutAction : INomisAction<int, bool>
    {
        private readonly IWorkoutRepository _workoutRepo;

        public DeleteWorkoutAction(IWorkoutRepository workoutRepo)
        {
            _workoutRepo = workoutRepo;
        }

        public async Task<(bool ok, string? error)> CanExecuteAsync(int id, CancellationToken ct = default)
        {
            var exists = await _workoutRepo.FirstOrDefaultAsync(new WorkoutByIdSpec(id));
            return exists is null ? (false, "Workout not found") : (true, null);
        }

        public async Task<bool> ExecuteAsync(int id, CancellationToken ct = default)
        {
            var w = await _workoutRepo.FirstOrDefaultAsync(new WorkoutByIdSpec(id));
            if (w is null) return false;
            _workoutRepo.Remove(w);
            await _workoutRepo.SaveChangesAsync();
            return true;
        }
    }
}