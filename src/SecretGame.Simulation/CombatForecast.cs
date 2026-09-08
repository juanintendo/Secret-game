namespace SecretGame.Simulation;

public sealed record ForecastResult(
    bool IsLegal,
    string? RejectionReason,
    IReadOnlyList<ResolvedEvent> Events,
    string ResultingStateHash);

public sealed class CombatForecast
{
    private readonly CombatResolver _resolver;

    public CombatForecast(CombatResolver resolver) => _resolver = resolver;

    public ForecastResult Evaluate(CombatState state, CombatCommand command)
    {
        var speculativeState = state.Clone();
        try
        {
            var result = _resolver.Resolve(speculativeState, command);
            return new ForecastResult(true, null, result.Events, result.ResultingStateHash);
        }
        catch (CommandRejectedException error)
        {
            return new ForecastResult(false, error.Message, Array.Empty<ResolvedEvent>(), state.DeterministicHash());
        }
    }
}
