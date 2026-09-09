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
            BeginActivationCommand begin => ResolveBeginActivation(state, begin),
            EndActivationCommand end => ResolveEndActivation(state, end),
            ApplyConditionCommand condition => ResolveCondition(state, condition),
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
        RequireActive(state, entity.Id);
        if (!entity.Flags.Spatial || !entity.Flags.Selectable)
            throw new CommandRejectedException($"Entity {command.EntityId} cannot move in its current state.");
        RequireActionPoints(entity, 1);

        if (command.MaximumSteps < 1)
            throw new CommandRejectedException("Movement allowance must be positive.");
        var path = _pathfinder.FindPath(state, entity, command.Destination)
            ?? throw new CommandRejectedException("Destination is not reachable.");
        if (path.StepCount > command.MaximumSteps)
            throw new CommandRejectedException($"Destination requires {path.StepCount} steps.");

        return new CombatEvent[]
        {
            Spend(entity, 1),
            new EntityMovedEvent(entity.Id, entity.Anchor, command.Destination, path)
        };
    }

    private static IReadOnlyList<CombatEvent> ResolveDamage(CombatState state, DamageCommand command)
    {
        var source = state.Require(command.SourceId);
        var target = state.Require(command.TargetId);
        RequireActive(state, source.Id);
        if (!source.Flags.Selectable)
            throw new CommandRejectedException($"Source {command.SourceId} cannot act in its current state.");
        RequireActionPoints(source, 1);
        if (!target.Flags.Targetable)
            throw new CommandRejectedException($"Target {command.TargetId} cannot be targeted in its current state.");
        if (!new LineOfSight().CanSee(state, source, target))
            throw new CommandRejectedException($"Source {command.SourceId} has no line of sight to {command.TargetId}.");
        if (command.Amount < 1)
            throw new CommandRejectedException("Damage amount must be positive.");

        var after = target.Integrity.ApplyDamage(command.Amount).Current;
        return new CombatEvent[]
        {
            Spend(source, 1),
            new IntegrityDamagedEvent(source.Id, target.Id, command.Amount, target.Integrity.Current, after)
        };
    }

    private static IReadOnlyList<CombatEvent> ResolveDeployment(
        CombatState state,
        SetDeploymentModeCommand command)
    {
        state.Require(command.PilotId);
        state.Require(command.MechId);
        RequireActive(state, command.PilotId);
        if (command.PilotId == command.MechId)
            throw new CommandRejectedException("Pilot and mech must have distinct stable IDs.");

        return new CombatEvent[]
        {
            new DeploymentModeChangedEvent(command.PilotId, command.MechId, command.Mode)
        };
    }

    private static IReadOnlyList<CombatEvent> ResolveBeginActivation(
        CombatState state,
        BeginActivationCommand command)
    {
        var entity = state.Require(command.EntityId);
        if (state.ActiveEntityId is not null)
            throw new CommandRejectedException($"Entity {state.ActiveEntityId.Value} is already active.");
        if (!entity.Flags.HasInitiativeSlot)
            throw new CommandRejectedException($"Entity {command.EntityId} has no initiative slot.");
        var next = new InitiativeTimeline().Query(state, 1).Single();
        if (next.EntityId != entity.Id)
            throw new CommandRejectedException($"Entity {command.EntityId} is not next in initiative.");
        var events = new List<CombatEvent>
        {
            new ActionPointsRefreshedEvent(
                entity.Id,
                entity.ActionPoints,
                2,
                entity.NextActAt,
                entity.NextActAt + entity.ActionInterval)
        };
        if (entity.Conditions.Items.Count > 0)
            events.Add(new ConditionsAdvancedEvent(
                entity.Id,
                entity.Conditions,
                entity.Conditions.AdvanceActivation()));
        return events;
    }

    private static IReadOnlyList<CombatEvent> ResolveEndActivation(
        CombatState state,
        EndActivationCommand command)
    {
        state.Require(command.EntityId);
        RequireActive(state, command.EntityId);
        return new CombatEvent[] { new ActivationEndedEvent(command.EntityId) };
    }

    private static IReadOnlyList<CombatEvent> ResolveCondition(
        CombatState state,
        ApplyConditionCommand command)
    {
        var source = state.Require(command.SourceId);
        var target = state.Require(command.TargetId);
        RequireActive(state, source.Id);
        if (!source.Flags.Selectable)
            throw new CommandRejectedException($"Source {command.SourceId} cannot act in its current state.");
        if (!target.Flags.Targetable)
            throw new CommandRejectedException($"Target {command.TargetId} cannot be targeted in its current state.");
        if (!new LineOfSight().CanSee(state, source, target))
            throw new CommandRejectedException($"Source {command.SourceId} has no line of sight to {command.TargetId}.");
        RequireActionPoints(source, 1);
        if (command.Duration < 1)
            throw new CommandRejectedException("Condition duration must be positive.");
        if (ConditionRules.IsDerived(command.Kind))
            throw new CommandRejectedException("Derived conditions cannot be applied or stored.");

        return new CombatEvent[]
        {
            Spend(source, 1),
            new ConditionAppliedEvent(source.Id, target.Id, command.Kind, command.Duration)
        };
    }

    private static void RequireActionPoints(CombatEntity entity, int amount)
    {
        if (entity.ActionPoints < amount)
            throw new CommandRejectedException($"Entity {entity.Id} needs {amount} action point.");
    }

    private static void RequireActive(CombatState state, EntityId entityId)
    {
        if (state.ActiveEntityId != entityId)
            throw new CommandRejectedException($"Entity {entityId} does not own the current activation.");
    }

    private static ActionPointsSpentEvent Spend(CombatEntity entity, int amount) =>
        new(entity.Id, amount, entity.ActionPoints, entity.ActionPoints - amount);
}
