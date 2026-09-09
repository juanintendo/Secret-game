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

public sealed record GuardGrantedEvent(
    EntityId SourceId,
    EntityId TargetId,
    int Amount,
    int Before,
    int After) : CombatEvent;

public sealed record GuardDamagedEvent(
    EntityId SourceId,
    EntityId TargetId,
    int Amount,
    int Before,
    int After) : CombatEvent;

public sealed record DeploymentModeChangedEvent(
    EntityId PilotId,
    EntityId MechId,
    DeploymentMode Mode,
    Cell? PilotDestination = null) : CombatEvent;

public sealed record ResourceChangedEvent(
    EntityId SourceId,
    ResourceId ResourceId,
    string ResourceName,
    ResourceChangeReason Reason,
    int Amount,
    int Before,
    int After) : CombatEvent;

public sealed record ActionPointsSpentEvent(
    EntityId EntityId,
    int Amount,
    int Before,
    int After) : CombatEvent;

public sealed record ActionPointsRefreshedEvent(
    EntityId EntityId,
    int Before,
    int After,
    int PreviousNextActAt,
    int NewNextActAt) : CombatEvent;

public sealed record ActivationEndedEvent(EntityId EntityId) : CombatEvent;

public sealed record ConditionAppliedEvent(
    EntityId SourceId,
    EntityId TargetId,
    ConditionKind Kind,
    int Duration) : CombatEvent;

public sealed record ConditionsAdvancedEvent(
    EntityId EntityId,
    ConditionSet Before,
    ConditionSet After) : CombatEvent;

public enum DisplacementStop
{
    Completed,
    Blocked,
    Occupied,
    Resisted
}

public sealed record EntityDisplacedEvent(
    EntityId SourceId,
    EntityId TargetId,
    Cell From,
    Cell To,
    CellPath Path,
    DisplacementStop Stop) : CombatEvent;

public sealed record ReactionChargeRefreshedEvent(
    EntityId EntityId,
    int Before,
    int After) : CombatEvent;

public sealed record ReactionChargeSpentEvent(
    EntityId EntityId,
    int Before,
    int After) : CombatEvent;

public sealed record ReactionTriggeredEvent(
    EntityId ReactorId,
    string ReactionId,
    int TriggerEffectIndex) : CombatEvent;

public sealed record DamageRedirectedEvent(
    EntityId ReactorId,
    EntityId OriginalTargetId,
    EntityId RedirectedTargetId) : CombatEvent;

public sealed record ResolvedEvent(
    long Sequence,
    CombatEvent Payload,
    string StateHash);
