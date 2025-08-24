namespace VortexCombat.Application.Actions.Nomis
{
    public interface INomisAction<TRequest, TResponse>
    {
        Task<(bool ok, string? error)> CanExecuteAsync(TRequest request, CancellationToken ct = default);
        Task<TResponse> ExecuteAsync(TRequest request, CancellationToken ct = default);
    }
}