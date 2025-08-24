using VortexCombat.Application.Specifications;
using VortexCombat.Domain.Entities;
using VortexCombat.Domain.Interfaces;

namespace VortexCombat.Application.Actions.Nomis
{
    public sealed class UpdateWorkoutAction : INomisAction<UpdateWorkoutRequest, Workout?>
    {
        private readonly IWorkoutRepository _workoutRepo;

        public UpdateWorkoutAction(IWorkoutRepository workoutRepo)
        {
            _workoutRepo = workoutRepo;
        }

        public async Task<(bool ok, string? error)> CanExecuteAsync(UpdateWorkoutRequest req, CancellationToken ct = default)
        {
            if (req.StartDate >= req.EndDate) return (false, "Start date must be before end date");
            var exists = await _workoutRepo.FirstOrDefaultAsync(new WorkoutByIdSpec(req.Id));
            return exists is null ? (false, "Workout not found") : (true, null);
        }

        public async Task<Workout?> ExecuteAsync(UpdateWorkoutRequest req, CancellationToken ct = default)
        {
            var w = await _workoutRepo.FirstOrDefaultAsync(new WorkoutByIdSpec(req.Id));
            if (w is null) return null;

            w.Description = req.Description;
            w.StartDate = req.StartDate;
            w.EndDate = req.EndDate;
            w.Room = req.Room;

            _workoutRepo.Update(w);
            await _workoutRepo.SaveChangesAsync();
            return w;
        }
    }
}