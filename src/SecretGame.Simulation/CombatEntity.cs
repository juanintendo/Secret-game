namespace SecretGame.Simulation;

public sealed record CombatEntity(
    EntityId Id,
    Cell Anchor,
    Footprint Footprint,
    EntityFlags Flags,
    IntegrityPool Integrity,
    int NextActAt = 0,
    int ActionInterval = 10)
{
    public Faction Faction { get; init; } = Faction.Player;
    public int ActionPoints { get; init; } = 2;
    public int ReactionCharges { get; init; } = 1;
    public ConditionSet Conditions { get; init; } = ConditionSet.Empty;
}
