namespace SecretGame.Simulation;

public abstract record CombatCommand;

public sealed record MoveCommand(EntityId EntityId, Cell Destination, int MaximumSteps = int.MaxValue) : CombatCommand;

public sealed record DamageCommand(EntityId SourceId, EntityId TargetId, int Amount) : CombatCommand;

public sealed record SetDeploymentModeCommand(
    EntityId PilotId,
    EntityId MechId,
    DeploymentMode Mode) : CombatCommand;

public sealed record BoardMechCommand(
    EntityId PilotId,
    EntityId MechId,
    int CostAp = 1) : CombatCommand;

public sealed record RechargeMechCommand(
    EntityId PilotId,
    EntityId MechId,
    ResourceId ResourceId,
    int Amount,
    int CostAp = 1) : CombatCommand;

public sealed record DeployMechRemotelyCommand(
    EntityId PilotId,
    EntityId MechId,
    Cell PilotDestination,
    ResourceId ResourceId,
    int ResourceCost,
    int CostAp = 1) : CombatCommand;

public sealed record RemoteMoveDirectiveCommand(
    EntityId PilotId,
    EntityId MechId,
    ResourceId ResourceId,
    Cell Destination,
    int MaximumSteps,
    int ResourceCost,
    int CostAp = 1) : CombatCommand;

public sealed record RemoteAttackDirectiveCommand(
    EntityId PilotId,
    EntityId MechId,
    ResourceId ResourceId,
    EntityId TargetId,
    int Damage,
    int ResourceCost,
    int CostAp = 1) : CombatCommand;

public sealed record BeginActivationCommand(EntityId EntityId) : CombatCommand;

public sealed record EndActivationCommand(EntityId EntityId) : CombatCommand;

public sealed record ApplyConditionCommand(
    EntityId SourceId,
    EntityId TargetId,
    ConditionKind Kind,
    int Duration) : CombatCommand;

public sealed record EffectStackCommand(
    EntityId SourceId,
    int CostAp,
    IReadOnlyList<CombatEffect> Effects,
    IReadOnlyList<ReactionInvocation>? Reactions = null) : CombatCommand;
