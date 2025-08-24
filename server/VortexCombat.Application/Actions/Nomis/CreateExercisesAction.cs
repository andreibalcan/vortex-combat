using VortexCombat.Domain.Entities;
using VortexCombat.Domain.Interfaces;

namespace VortexCombat.Application.Actions.Nomis
{
    public sealed class CreateExercisesAction : INomisAction<List<Exercise>, int>
    {
        private readonly IExerciseRepository _exerciseRepo;

        public CreateExercisesAction(IExerciseRepository exerciseRepo)
        {
            _exerciseRepo = exerciseRepo;
        }

        public Task<(bool ok, string? error)> CanExecuteAsync(List<Exercise> request, CancellationToken ct = default)
        {
            if (request is null || request.Count == 0)
                return Task.FromResult((false, "No exercises provided"));
            return Task.FromResult((true, (string?)null));
        }

        public async Task<int> ExecuteAsync(List<Exercise> request, CancellationToken ct = default)
        {
            await _exerciseRepo.AddRangeAsync(request);
            await _exerciseRepo.SaveChangesAsync();
            return request.Count;
        }
    }
}