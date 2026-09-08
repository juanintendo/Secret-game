namespace SecretGame.Simulation;

public sealed record ReplayResult(
    IReadOnlyList<ResolvedEvent> Events,
    string FinalStateHash,
    CombatState FinalState);

public sealed class CombatReplay
{
    private readonly CombatResolver _resolver;

    public CombatReplay(CombatResolver resolver) => _resolver = resolver;

    public ReplayResult Run(CombatState initialState, IEnumerable<CombatCommand> commands)
    {
        var state = initialState.Clone();
        var events = new List<ResolvedEvent>();
        foreach (var command in commands)
            events.AddRange(_resolver.Resolve(state, command).Events);

        return new ReplayResult(events, state.DeterministicHash(), state);
    }
}
