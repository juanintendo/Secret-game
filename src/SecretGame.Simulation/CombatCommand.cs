namespace SecretGame.Simulation;

public abstract record CombatCommand;

public sealed record MoveCommand(EntityId EntityId, Cell Destination, int MaximumSteps = int.MaxValue) : CombatCommand;

public sealed record DamageCommand(EntityId SourceId, EntityId TargetId, int Amount) : CombatCommand;

public sealed record SetDeploymentModeCommand(
    EntityId PilotId,
    EntityId MechId,
    DeploymentMode Mode) : CombatCommand;

public sealed record BeginActivationCommand(EntityId EntityId) : CombatCommand;

public sealed record EndActivationCommand(EntityId EntityId) : CombatCommand;

public sealed record ApplyConditionCommand(
    EntityId SourceId,
    EntityId TargetId,
    ConditionKind Kind,
    int Duration) : CombatCommand;
