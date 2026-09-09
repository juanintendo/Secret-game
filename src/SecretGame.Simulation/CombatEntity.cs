namespace SecretGame.Simulation;

public sealed record CombatEntity(
    EntityId Id,
    Cell Anchor,
    Footprint Footprint,
    EntityFlags Flags,
    IntegrityPool Integrity,
    int NextActAt = 0,
    int ActionInterval = 10);
