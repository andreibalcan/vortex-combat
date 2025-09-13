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
            var workout = await _workoutRepo.FirstOrDefaultAsync(new WorkoutByIdSpec(req.Id));
            if (workout is null) return null;

            workout.Description = req.Description;
            workout.StartDate = req.StartDate;
            workout.EndDate = req.EndDate;
            workout.Room = req.Room;

            await _workoutRepo.UpdateWorkoutExercisesAsync(req.Id, req.Exercises);

            _workoutRepo.Update(workout);
            await _workoutRepo.SaveChangesAsync();
            return workout;
        }
    }
}