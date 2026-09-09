namespace SecretGame.Simulation;

public abstract record CombatCommand;

public sealed record MoveCommand(EntityId EntityId, Cell Destination, int MaximumSteps = int.MaxValue) : CombatCommand;

public sealed record DamageCommand(EntityId SourceId, EntityId TargetId, int Amount) : CombatCommand;

public sealed record SetDeploymentModeCommand(
    EntityId PilotId,
    EntityId MechId,
    DeploymentMode Mode) : CombatCommand;
