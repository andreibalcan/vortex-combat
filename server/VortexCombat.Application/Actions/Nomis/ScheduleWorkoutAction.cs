using VortexCombat.Domain.Entities;
using VortexCombat.Domain.Interfaces;

namespace VortexCombat.Application.Actions.Nomis
{
    public sealed class ScheduleWorkoutAction : INomisAction<ScheduleWorkoutRequest, Workout>
    {
        private readonly IWorkoutRepository _workoutRepo;

        public ScheduleWorkoutAction(IWorkoutRepository workoutRepo)
        {
            _workoutRepo = workoutRepo;
        }

        public Task<(bool ok, string? error)> CanExecuteAsync(ScheduleWorkoutRequest req,
            CancellationToken ct = default)
        {
            if (req.StartDate >= req.EndDate) return Task.FromResult((false, "Start date must be before end date"));
            if (string.IsNullOrWhiteSpace(req.Room)) return Task.FromResult((false, "Room is required"));
            return Task.FromResult((true, (string?)null));
        }

        public async Task<Workout> ExecuteAsync(ScheduleWorkoutRequest req, CancellationToken ct = default)
        {
            var workout = new Workout
            {
                Description = req.Description,
                StartDate = req.StartDate,
                EndDate = req.EndDate,
                Room = req.Room
            };

            await _workoutRepo.AddAsync(workout);
            await _workoutRepo.SaveChangesAsync();

            foreach (var exId in req.Exercises.Distinct())
                workout.WorkoutExercises.Add(new WorkoutExercise { WorkoutId = workout.Id, ExerciseId = exId });

            await _workoutRepo.SaveChangesAsync();
            return workout;
        }
    }
}