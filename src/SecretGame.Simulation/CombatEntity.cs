namespace SecretGame.Simulation;

public sealed record CombatEntity(
    EntityId Id,
    Cell Anchor,
    Footprint Footprint,
    EntityFlags Flags,
    IntegrityPool Integrity);
