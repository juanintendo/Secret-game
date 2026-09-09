namespace SecretGame.Simulation;

public sealed record ForecastResult(
    bool IsLegal,
    string? RejectionReason,
    IReadOnlyList<ResolvedEvent> Events,
    string ResultingStateHash,
    IReadOnlyList<ConditionSignal> ConditionsOpened,
    IReadOnlyList<ConditionSignal> ConditionsClosed);

public sealed class CombatForecast
{
    private readonly CombatResolver _resolver;
    private readonly ConditionEvaluator _conditions = new();

    public CombatForecast(CombatResolver resolver) => _resolver = resolver;

    public ForecastResult Evaluate(CombatState state, CombatCommand command)
    {
        var before = _conditions.Evaluate(state);
        var speculativeState = state.Clone();
        try
        {
            var result = _resolver.Resolve(speculativeState, command);
            var after = _conditions.Evaluate(speculativeState);
            return new ForecastResult(
                true,
                null,
                result.Events,
                result.ResultingStateHash,
                after.Except(before).OrderBy(signal => signal).ToArray(),
                before.Except(after).OrderBy(signal => signal).ToArray());
        }
        catch (CommandRejectedException error)
        {
            return new ForecastResult(
                false,
                error.Message,
                Array.Empty<ResolvedEvent>(),
                state.DeterministicHash(),
                Array.Empty<ConditionSignal>(),
                Array.Empty<ConditionSignal>());
        }
    }
}
