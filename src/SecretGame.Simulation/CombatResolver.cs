namespace SecretGame.Simulation;

public sealed class CombatResolver
{
    private readonly GridPathfinder _pathfinder = new();

    public CommandResult Resolve(CombatState state, CombatCommand command)
    {
        ArgumentNullException.ThrowIfNull(state);
        ArgumentNullException.ThrowIfNull(command);

        if (command is EffectStackCommand stack)
            return ResolveEffectStack(state, stack);

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

    private CommandResult ResolveEffectStack(CombatState state, EffectStackCommand command)
    {
        ResolveEffectStackUnchecked(state.Clone(), command);
        return ResolveEffectStackUnchecked(state, command);
    }

    private CommandResult ResolveEffectStackUnchecked(CombatState state, EffectStackCommand command)
    {
        var source = state.Require(command.SourceId);
        RequireActive(state, source.Id);
        if (!source.Flags.Selectable)
            throw new CommandRejectedException($"Source {source.Id} cannot act in its current state.");
        if (command.CostAp is < 1 or > 2)
            throw new CommandRejectedException("Effect stack AP cost must be one or two.");
        RequireActionPoints(source, command.CostAp);
        var reactions = command.Reactions ?? Array.Empty<ReactionInvocation>();
        var totalApplications = command.Effects.Count + reactions.Sum(item => item.ResponseEffects.Count);
        if (command.Effects.Count < 1 || totalApplications > 8)
            throw new CommandRejectedException("Effect stack must contain between one and eight effects.");
        ValidateReactionInvocations(command.Effects.Count, reactions);

        var events = new List<ResolvedEvent> { state.Apply(Spend(source, command.CostAp)) };
        for (var effectIndex = 0; effectIndex < command.Effects.Count; effectIndex++)
        {
            ApplyEffect(state, command.SourceId, command.Effects[effectIndex], events);
            foreach (var reaction in reactions.Where(item => item.TriggerEffectIndex == effectIndex)
                         .OrderBy(item => item.ReactorId))
            {
                var reactor = state.Require(reaction.ReactorId);
                if (reactor.ReactionCharges < 1)
                    throw new CommandRejectedException($"Reactor {reactor.Id} has no reaction charge.");
                events.Add(state.Apply(new ReactionChargeSpentEvent(reactor.Id, reactor.ReactionCharges, reactor.ReactionCharges - 1)));
                events.Add(state.Apply(new ReactionTriggeredEvent(reactor.Id, reaction.ReactionId, effectIndex)));
                foreach (var response in reaction.ResponseEffects)
                    ApplyEffect(state, reactor.Id, response, events);
            }
        }
        return new CommandResult(events, state.DeterministicHash());
    }

    private static void ApplyEffect(
        CombatState state,
        EntityId sourceId,
        CombatEffect effect,
        List<ResolvedEvent> events)
    {
        var payloads = effect switch
        {
            DamageEffect damage => ResolveEffectDamage(state, sourceId, damage),
            ApplyConditionEffect condition => ResolveEffectCondition(state, sourceId, condition),
            DisplaceEffect displace => ResolveDisplace(state, sourceId, displace),
            _ => throw new CommandRejectedException($"Unsupported effect type {effect.GetType().Name}.")
        };
        foreach (var payload in payloads) events.Add(state.Apply(payload));
    }

    private static void ValidateReactionInvocations(
        int effectCount,
        IReadOnlyList<ReactionInvocation> reactions)
    {
        foreach (var reaction in reactions)
        {
            if (reaction.TriggerEffectIndex < 0 || reaction.TriggerEffectIndex >= effectCount)
                throw new CommandRejectedException($"Reaction {reaction.ReactionId} references an absent trigger effect.");
            if (reaction.ResponseEffects.Count == 0)
                throw new CommandRejectedException($"Reaction {reaction.ReactionId} has no response effect.");
        }
        foreach (var group in reactions.GroupBy(item => item.TriggerEffectIndex))
        {
            if (group.Count() > 2)
                throw new CommandRejectedException("At most two reactions may resolve for one effect.");
            if (group.GroupBy(item => item.ReactorId).Any(owners => owners.Count() > 1))
                throw new CommandRejectedException("A unit may react at most once to one effect.");
        }
    }

    private static IReadOnlyList<CombatEvent> ResolveEffectDamage(
        CombatState state,
        EntityId sourceId,
        DamageEffect effect)
    {
        var source = state.Require(sourceId);
        var target = state.Require(effect.TargetId);
        RequireVisibleTarget(state, source, target);
        if (effect.Amount < 1) throw new CommandRejectedException("Damage amount must be positive.");
        var after = target.Integrity.ApplyDamage(effect.Amount).Current;
        return new CombatEvent[]
        {
            new IntegrityDamagedEvent(sourceId, target.Id, effect.Amount, target.Integrity.Current, after)
        };
    }

    private static IReadOnlyList<CombatEvent> ResolveEffectCondition(
        CombatState state,
        EntityId sourceId,
        ApplyConditionEffect effect)
    {
        var source = state.Require(sourceId);
        var target = state.Require(effect.TargetId);
        RequireVisibleTarget(state, source, target);
        if (effect.Duration < 1) throw new CommandRejectedException("Condition duration must be positive.");
        if (ConditionRules.IsDerived(effect.Kind))
            throw new CommandRejectedException("Derived conditions cannot be applied or stored.");
        return new CombatEvent[] { new ConditionAppliedEvent(sourceId, target.Id, effect.Kind, effect.Duration) };
    }

    private static IReadOnlyList<CombatEvent> ResolveDisplace(
        CombatState state,
        EntityId sourceId,
        DisplaceEffect effect)
    {
        var target = state.Require(effect.TargetId);
        if (!target.Flags.Spatial || !target.Flags.Targetable)
            throw new CommandRejectedException($"Target {target.Id} cannot be displaced.");
        if (effect.Distance < 1) throw new CommandRejectedException("Displacement distance must be positive.");
        if (effect.ImpactDamage < 0) throw new CommandRejectedException("Impact damage cannot be negative.");

        var step = effect.Direction.Step();
        var cells = new List<Cell> { target.Anchor };
        var current = target.Anchor;
        var stop = DisplacementStop.Completed;
        CombatEntity? collided = null;
        for (var index = 0; index < effect.Distance; index++)
        {
            var next = new Cell(current.X + step.X, current.Y + step.Y);
            var occupied = target.Footprint.OccupiedCells(next).ToArray();
            if (occupied.Any(cell => !state.Map.Contains(cell) || state.Map.GetTerrain(cell).Blocked))
            {
                stop = DisplacementStop.Blocked;
                break;
            }
            collided = state.Entities.Values
                .Where(entity => entity.Id != target.Id && entity.Flags.Spatial)
                .OrderBy(entity => entity.Id)
                .FirstOrDefault(entity => entity.Footprint.OccupiedCells(entity.Anchor).Intersect(occupied).Any());
            if (collided is not null)
            {
                stop = DisplacementStop.Occupied;
                break;
            }
            current = next;
            cells.Add(current);
        }

        var events = new List<CombatEvent>
        {
            new EntityDisplacedEvent(sourceId, target.Id, target.Anchor, current, new CellPath(cells), stop)
        };
        if (stop != DisplacementStop.Completed && effect.ImpactDamage > 0)
        {
            var targetAfter = target.Integrity.ApplyDamage(effect.ImpactDamage).Current;
            events.Add(new IntegrityDamagedEvent(sourceId, target.Id, effect.ImpactDamage, target.Integrity.Current, targetAfter));
            if (collided is not null)
            {
                var collidedAfter = collided.Integrity.ApplyDamage(effect.ImpactDamage).Current;
                events.Add(new IntegrityDamagedEvent(sourceId, collided.Id, effect.ImpactDamage, collided.Integrity.Current, collidedAfter));
            }
        }
        return events;
    }

    private static void RequireVisibleTarget(CombatState state, CombatEntity source, CombatEntity target)
    {
        if (!target.Flags.Targetable)
            throw new CommandRejectedException($"Target {target.Id} cannot be targeted in its current state.");
        if (!new LineOfSight().CanSee(state, source, target))
            throw new CommandRejectedException($"Source {source.Id} has no line of sight to {target.Id}.");
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
        events.Add(new ReactionChargeRefreshedEvent(entity.Id, entity.ReactionCharges, 1));
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
