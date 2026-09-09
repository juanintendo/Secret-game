namespace SecretGame.Simulation;

public sealed class CombatResolver
{
    private readonly GridPathfinder _pathfinder = new();

    public CommandResult Resolve(CombatState state, CombatCommand command)
    {
        if (state is null) throw new ArgumentNullException(nameof(state));
        if (command is null) throw new ArgumentNullException(nameof(command));

        if (command is EffectStackCommand stack)
            return ResolveEffectStack(state, stack);

        var payloads = command switch
        {
            MoveCommand move => ResolveMove(state, move),
            DamageCommand damage => ResolveDamage(state, damage),
            SetDeploymentModeCommand deployment => ResolveDeployment(state, deployment),
            BoardMechCommand board => ResolveBoardMech(state, board),
            RechargeMechCommand recharge => ResolveRechargeMech(state, recharge),
            DeployMechRemotelyCommand deploy => ResolveDeployMechRemotely(state, deploy),
            RemoteMoveDirectiveCommand remoteMove => ResolveRemoteMove(state, remoteMove),
            RemoteAttackDirectiveCommand remoteAttack => ResolveRemoteAttack(state, remoteAttack),
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
            var effect = ApplyReactions(state, command.Effects[effectIndex], effectIndex, ReactionTiming.Before, reactions, events);
            ApplyEffect(state, command.SourceId, effect, events);
            ApplyReactions(state, effect, effectIndex, ReactionTiming.After, reactions, events);
        }
        return new CommandResult(events, state.DeterministicHash());
    }

    private static CombatEffect ApplyReactions(
        CombatState state,
        CombatEffect triggeringEffect,
        int effectIndex,
        ReactionTiming timing,
        IReadOnlyList<ReactionInvocation> reactions,
        List<ResolvedEvent> events)
    {
        var effect = triggeringEffect;
        foreach (var reaction in reactions.Where(item => item.TriggerEffectIndex == effectIndex && item.Timing == timing)
                     .OrderBy(item => item.ReactorId))
        {
            var reactor = state.Require(reaction.ReactorId);
            if (reactor.ReactionCharges < 1)
                throw new CommandRejectedException($"Reactor {reactor.Id} has no reaction charge.");
            events.Add(state.Apply(new ReactionChargeSpentEvent(reactor.Id, reactor.ReactionCharges, reactor.ReactionCharges - 1)));
            events.Add(state.Apply(new ReactionTriggeredEvent(reactor.Id, reaction.ReactionId, effectIndex)));
            if (reaction.RedirectTargetId is not null)
            {
                if (timing != ReactionTiming.Before || effect is not DamageEffect damage)
                    throw new CommandRejectedException("Only a before-damage reaction may redirect a target.");
                events.Add(state.Apply(new DamageRedirectedEvent(
                    reactor.Id,
                    damage.TargetId,
                    reaction.RedirectTargetId.Value)));
                effect = damage with { TargetId = reaction.RedirectTargetId.Value };
            }
            foreach (var response in reaction.ResponseEffects)
                ApplyEffect(state, reactor.Id, response, events);
        }
        return effect;
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
            GrantGuardEffect guard => ResolveGrantGuard(state, sourceId, guard),
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
            if (reaction.ResponseEffects.Count == 0 && reaction.Timing != ReactionTiming.Before)
                throw new CommandRejectedException($"Reaction {reaction.ReactionId} has no response effect.");
        }
        foreach (var group in reactions.GroupBy(item => item.TriggerEffectIndex))
        {
            if (group.Count() > 2)
                throw new CommandRejectedException("At most two reactions may resolve for one effect.");
            if (group.GroupBy(item => item.ReactorId).Any(owners => owners.Count() > 1))
                throw new CommandRejectedException("A unit may react at most once to one effect.");
            if (group.Count(item => item.RedirectTargetId is not null) > 1)
                throw new CommandRejectedException("At most one reaction may redirect one effect.");
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
        return BuildDamageEvents(sourceId, target, effect.Amount, effect.Piercing);
    }

    private static IReadOnlyList<CombatEvent> ResolveGrantGuard(
        CombatState state,
        EntityId sourceId,
        GrantGuardEffect effect)
    {
        var target = state.Require(effect.TargetId);
        if (!target.Flags.Targetable) throw new CommandRejectedException($"Target {target.Id} cannot receive Guard.");
        if (effect.Amount < 1) throw new CommandRejectedException("Guard amount must be positive.");
        return new CombatEvent[]
        {
            new GuardGrantedEvent(sourceId, target.Id, effect.Amount, target.Guard.Current, target.Guard.Current + effect.Amount)
        };
    }

    private static IReadOnlyList<CombatEvent> BuildDamageEvents(
        EntityId sourceId,
        CombatEntity target,
        int amount,
        bool piercing)
    {
        var events = new List<CombatEvent>();
        var absorbed = piercing ? 0 : Math.Min(target.Guard.Current, amount);
        if (absorbed > 0)
            events.Add(new GuardDamagedEvent(sourceId, target.Id, absorbed, target.Guard.Current, target.Guard.Current - absorbed));
        var remaining = amount - absorbed;
        if (remaining > 0)
        {
            var after = target.Integrity.ApplyDamage(remaining).Current;
            events.Add(new IntegrityDamagedEvent(sourceId, target.Id, remaining, target.Integrity.Current, after));
        }
        return events;
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

        var effectiveDistance = target.Mass switch
        {
            MassClass.Light => effect.Distance,
            MassClass.Standard => Math.Max(0, effect.Distance - 1),
            MassClass.Heavy => Math.Max(0, effect.Distance - 2),
            MassClass.Anchored => 0,
            _ => throw new CommandRejectedException("Unknown mass class.")
        };
        var step = effect.Direction.Step();
        var cells = new List<Cell> { target.Anchor };
        var current = target.Anchor;
        var stop = effectiveDistance == 0 ? DisplacementStop.Resisted : DisplacementStop.Completed;
        CombatEntity? collided = null;
        for (var index = 0; index < effectiveDistance; index++)
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
        if (target.Mass == MassClass.Anchored)
            events.Add(new ConditionAppliedEvent(sourceId, target.Id, ConditionKind.Staggered, 1));
        if (stop is DisplacementStop.Blocked or DisplacementStop.Occupied && effect.ImpactDamage > 0)
        {
            events.AddRange(BuildDamageEvents(sourceId, target, effect.ImpactDamage, false));
            if (collided is not null)
                events.AddRange(BuildDamageEvents(sourceId, collided, effect.ImpactDamage, false));
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

        return new CombatEvent[] { Spend(source, 1) }
            .Concat(BuildDamageEvents(source.Id, target, command.Amount, false))
            .ToArray();
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

    private static IReadOnlyList<CombatEvent> ResolveBoardMech(
        CombatState state,
        BoardMechCommand command)
    {
        var pilot = RequirePilotMechPair(state, command.PilotId, command.MechId);
        var mech = state.Require(command.MechId);
        state.RequireLinkedPair(pilot.Id, mech.Id);
        RequireActive(state, pilot.Id);
        RequireActionCost(command.CostAp, "Board Mech");
        RequireActionPoints(pilot, command.CostAp);
        if (!pilot.Flags.Spatial || !pilot.Flags.Selectable)
            throw new CommandRejectedException("Pilot must be deployed to Board Mech.");
        if (!mech.Flags.Spatial || !mech.Flags.Selectable)
            throw new CommandRejectedException("Mech must be deployed to receive its pilot.");
        if (!AreAdjacent(pilot, mech))
            throw new CommandRejectedException("Pilot must be adjacent to the mech to board it.");

        return new CombatEvent[]
        {
            Spend(pilot, command.CostAp),
            new DeploymentModeChangedEvent(pilot.Id, mech.Id, DeploymentMode.Docked)
        };
    }

    private static IReadOnlyList<CombatEvent> ResolveRechargeMech(
        CombatState state,
        RechargeMechCommand command)
    {
        var pilot = RequirePilotMechPair(state, command.PilotId, command.MechId);
        var mech = state.Require(command.MechId);
        var resource = state.RequireResource(command.ResourceId);
        RequireResourcePair(resource, pilot.Id, mech.Id);
        RequireActive(state, mech.Id);
        RequireActionCost(command.CostAp, "Recharge");
        RequireActionPoints(mech, command.CostAp);
        if (pilot.Flags != EntityFlags.Docked)
            throw new CommandRejectedException("Recharge requires the pilot to be aboard the mech.");
        if (command.Amount < 1)
            throw new CommandRejectedException("Recharge amount must be positive.");
        if (resource.Current >= resource.Maximum)
            throw new CommandRejectedException($"{resource.Name} is already full.");
        var remainingCapacity = resource.Maximum - resource.Current;
        var after = command.Amount >= remainingCapacity
            ? resource.Maximum
            : resource.Current + command.Amount;

        return new CombatEvent[]
        {
            Spend(mech, command.CostAp),
            new ResourceChangedEvent(mech.Id, resource.Id, resource.Name, ResourceChangeReason.Recharge,
                after - resource.Current, resource.Current, after)
        };
    }

    private static IReadOnlyList<CombatEvent> ResolveDeployMechRemotely(
        CombatState state,
        DeployMechRemotelyCommand command)
    {
        var pilot = RequirePilotMechPair(state, command.PilotId, command.MechId);
        var mech = state.Require(command.MechId);
        var resource = state.RequireResource(command.ResourceId);
        RequireResourcePair(resource, pilot.Id, mech.Id);
        RequireActive(state, mech.Id);
        RequireActionCost(command.CostAp, "Deploy Remotely");
        RequireActionPoints(mech, command.CostAp);
        if (pilot.Flags != EntityFlags.Docked)
            throw new CommandRejectedException("Deploy Remotely requires the pilot to begin aboard the mech.");
        if (command.ResourceCost < 1)
            throw new CommandRejectedException("Remote deployment resource cost must be positive.");
        if (resource.Current < command.ResourceCost)
            throw new CommandRejectedException($"Deploy Remotely needs {command.ResourceCost} {resource.Name}.");
        var deployedPilot = pilot with { Anchor = command.PilotDestination, Flags = EntityFlags.Active };
        if (!AreAdjacent(deployedPilot, mech))
            throw new CommandRejectedException("Pilot must deploy into a cell adjacent to the mech.");
        RequireLegalDeploymentCell(state, deployedPilot, mech.Id);

        return new CombatEvent[]
        {
            Spend(mech, command.CostAp),
            new ResourceChangedEvent(mech.Id, resource.Id, resource.Name, ResourceChangeReason.Deployment,
                -command.ResourceCost, resource.Current, resource.Current - command.ResourceCost),
            new DeploymentModeChangedEvent(pilot.Id, mech.Id, DeploymentMode.Remote, command.PilotDestination)
        };
    }

    private static CombatEntity RequirePilotMechPair(
        CombatState state,
        EntityId pilotId,
        EntityId mechId)
    {
        if (pilotId == mechId)
            throw new CommandRejectedException("Pilot and mech must have distinct stable IDs.");
        var pilot = state.Require(pilotId);
        state.Require(mechId);
        return pilot;
    }

    private IReadOnlyList<CombatEvent> ResolveRemoteMove(
        CombatState state,
        RemoteMoveDirectiveCommand command)
    {
        var pilot = RequirePilotMechPair(state, command.PilotId, command.MechId);
        var mech = state.Require(command.MechId);
        var resource = state.RequireResource(command.ResourceId);
        RequireRemoteDirective(state, pilot, mech, resource, command.CostAp, command.ResourceCost);
        if (command.MaximumSteps < 1)
            throw new CommandRejectedException("Remote movement allowance must be positive.");
        var path = _pathfinder.FindPath(state, mech, command.Destination)
            ?? throw new CommandRejectedException("Remote mech destination is not reachable.");
        if (path.StepCount > command.MaximumSteps)
            throw new CommandRejectedException($"Remote mech destination requires {path.StepCount} steps.");

        return new CombatEvent[]
        {
            Spend(pilot, command.CostAp),
            SpendResource(pilot.Id, resource, command.ResourceCost, ResourceChangeReason.RemoteDirective),
            new RemoteDirectiveIssuedEvent(pilot.Id, mech.Id, RemoteDirectiveKind.Move),
            new EntityMovedEvent(mech.Id, mech.Anchor, command.Destination, path)
        };
    }

    private static IReadOnlyList<CombatEvent> ResolveRemoteAttack(
        CombatState state,
        RemoteAttackDirectiveCommand command)
    {
        var pilot = RequirePilotMechPair(state, command.PilotId, command.MechId);
        var mech = state.Require(command.MechId);
        var resource = state.RequireResource(command.ResourceId);
        RequireRemoteDirective(state, pilot, mech, resource, command.CostAp, command.ResourceCost);
        var target = state.Require(command.TargetId);
        RequireVisibleTarget(state, mech, target);
        if (command.Damage < 1)
            throw new CommandRejectedException("Remote attack damage must be positive.");

        return new CombatEvent[]
        {
            Spend(pilot, command.CostAp),
            SpendResource(pilot.Id, resource, command.ResourceCost, ResourceChangeReason.RemoteDirective),
            new RemoteDirectiveIssuedEvent(pilot.Id, mech.Id, RemoteDirectiveKind.Attack)
        }.Concat(BuildDamageEvents(mech.Id, target, command.Damage, false)).ToArray();
    }

    private static void RequireRemoteDirective(
        CombatState state,
        CombatEntity pilot,
        CombatEntity mech,
        SharedResourcePool resource,
        int costAp,
        int resourceCost)
    {
        RequireResourcePair(resource, pilot.Id, mech.Id);
        RequireActive(state, pilot.Id);
        RequireActionCost(costAp, "Remote directive");
        RequireActionPoints(pilot, costAp);
        if (pilot.Flags != EntityFlags.Active || mech.Flags != EntityFlags.RemoteControlled)
            throw new CommandRejectedException("Remote directives require a deployed pilot and remote-controlled mech.");
        if (resourceCost < 1)
            throw new CommandRejectedException("Remote directive resource cost must be positive.");
        if (resource.Current < resourceCost)
            throw new CommandRejectedException($"Remote directive needs {resourceCost} {resource.Name}.");
    }

    private static ResourceChangedEvent SpendResource(
        EntityId sourceId,
        SharedResourcePool resource,
        int amount,
        ResourceChangeReason reason) =>
        new(sourceId, resource.Id, resource.Name, reason, -amount, resource.Current, resource.Current - amount);

    private static void RequireActionCost(int costAp, string actionName)
    {
        if (costAp is < 1 or > 2)
            throw new CommandRejectedException($"{actionName} AP cost must be one or two.");
    }

    private static void RequireResourcePair(SharedResourcePool resource, EntityId pilotId, EntityId mechId)
    {
        if (resource.OwnerId != pilotId || resource.PartnerId != mechId)
            throw new CommandRejectedException($"{resource.Name} does not belong to pilot/mech pair {pilotId}/{mechId}.");
    }

    private static bool AreAdjacent(CombatEntity first, CombatEntity second)
    {
        foreach (var firstCell in first.Footprint.OccupiedCells(first.Anchor))
        foreach (var secondCell in second.Footprint.OccupiedCells(second.Anchor))
        {
            var horizontal = Math.Abs(firstCell.X - secondCell.X);
            var vertical = Math.Abs(firstCell.Y - secondCell.Y);
            if (horizontal <= 1 && vertical <= 1 && horizontal + vertical > 0) return true;
        }
        return false;
    }

    private static void RequireLegalDeploymentCell(CombatState state, CombatEntity pilot, EntityId mechId)
    {
        var occupied = pilot.Footprint.OccupiedCells(pilot.Anchor).ToArray();
        if (occupied.Any(cell => !state.Map.Contains(cell) || state.Map.GetTerrain(cell).Blocked))
            throw new CommandRejectedException("Pilot deployment cell is blocked or outside the map.");
        var overlaps = state.Entities.Values
            .Where(entity => entity.Flags.Spatial && entity.Id != pilot.Id)
            .Any(entity => entity.Footprint.OccupiedCells(entity.Anchor).Intersect(occupied).Any());
        if (overlaps)
            throw new CommandRejectedException($"Pilot deployment cell overlaps {mechId} or another entity.");
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
