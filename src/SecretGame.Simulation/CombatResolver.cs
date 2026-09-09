namespace SecretGame.Simulation;

public sealed class CombatResolver
{
    private readonly GridPathfinder _pathfinder = new();

    public CommandResult Resolve(CombatState state, CombatCommand command)
    {
        ArgumentNullException.ThrowIfNull(state);
        ArgumentNullException.ThrowIfNull(command);

        var payloads = command switch
        {
            MoveCommand move => ResolveMove(state, move),
            DamageCommand damage => ResolveDamage(state, damage),
            SetDeploymentModeCommand deployment => ResolveDeployment(state, deployment),
            _ => throw new CommandRejectedException($"Unsupported command type {command.GetType().Name}.")
        };

        var events = new List<ResolvedEvent>(payloads.Count);
        foreach (var payload in payloads)
            events.Add(state.Apply(payload));

        return new CommandResult(events, state.DeterministicHash());
    }

    private IReadOnlyList<CombatEvent> ResolveMove(CombatState state, MoveCommand command)
    {
        var entity = state.Require(command.EntityId);
        if (!entity.Flags.Spatial || !entity.Flags.Selectable)
            throw new CommandRejectedException($"Entity {command.EntityId} cannot move in its current state.");

        if (command.MaximumSteps < 1)
            throw new CommandRejectedException("Movement allowance must be positive.");
        var path = _pathfinder.FindPath(state, entity, command.Destination)
            ?? throw new CommandRejectedException("Destination is not reachable.");
        if (path.StepCount > command.MaximumSteps)
            throw new CommandRejectedException($"Destination requires {path.StepCount} steps.");

        return new CombatEvent[] { new EntityMovedEvent(entity.Id, entity.Anchor, command.Destination, path) };
    }

    private static IReadOnlyList<CombatEvent> ResolveDamage(CombatState state, DamageCommand command)
    {
        var source = state.Require(command.SourceId);
        var target = state.Require(command.TargetId);
        if (!source.Flags.Selectable)
            throw new CommandRejectedException($"Source {command.SourceId} cannot act in its current state.");
        if (!target.Flags.Targetable)
            throw new CommandRejectedException($"Target {command.TargetId} cannot be targeted in its current state.");
        if (command.Amount < 1)
            throw new CommandRejectedException("Damage amount must be positive.");

        var after = target.Integrity.ApplyDamage(command.Amount).Current;
        return new CombatEvent[]
        {
            new IntegrityDamagedEvent(source.Id, target.Id, command.Amount, target.Integrity.Current, after)
        };
    }

    private static IReadOnlyList<CombatEvent> ResolveDeployment(
        CombatState state,
        SetDeploymentModeCommand command)
    {
        state.Require(command.PilotId);
        state.Require(command.MechId);
        if (command.PilotId == command.MechId)
            throw new CommandRejectedException("Pilot and mech must have distinct stable IDs.");

        return new CombatEvent[]
        {
            new DeploymentModeChangedEvent(command.PilotId, command.MechId, command.Mode)
        };
    }
}
