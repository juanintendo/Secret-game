namespace SecretGame.Simulation;

public abstract record CombatEvent;

public sealed record EntityMovedEvent(
    EntityId EntityId,
    Cell From,
    Cell To,
    CellPath Path) : CombatEvent;

public sealed record IntegrityDamagedEvent(
    EntityId SourceId,
    EntityId TargetId,
    int Amount,
    int Before,
    int After) : CombatEvent;

public sealed record DeploymentModeChangedEvent(
    EntityId PilotId,
    EntityId MechId,
    DeploymentMode Mode) : CombatEvent;

public sealed record ResolvedEvent(
    long Sequence,
    CombatEvent Payload,
    string StateHash);
