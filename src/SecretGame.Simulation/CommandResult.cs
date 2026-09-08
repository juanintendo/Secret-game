namespace SecretGame.Simulation;

public sealed record CommandResult(IReadOnlyList<ResolvedEvent> Events, string ResultingStateHash);
