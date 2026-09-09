namespace SecretGame.Simulation;

public enum MovementTopology
{
    CardinalFour,
    EightConnected
}

public sealed record CombatRules(MovementTopology MovementTopology, int MaximumClimb)
{
    public static CombatRules SpikeDefault { get; } = new(MovementTopology.EightConnected, 1);
}
