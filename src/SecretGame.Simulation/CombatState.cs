using System.Security.Cryptography;

namespace SecretGame.Simulation;

public sealed class CombatState
{
    private readonly SortedDictionary<EntityId, CombatEntity> _entities;
    private readonly List<ResolvedEvent> _events;

    public CombatState(
        BattleMap map,
        CombatRules rules,
        IEnumerable<CombatEntity> entities,
        EntityId? activeEntityId = null)
    {
        Map = map ?? throw new ArgumentNullException(nameof(map));
        Rules = rules ?? throw new ArgumentNullException(nameof(rules));
        _entities = new SortedDictionary<EntityId, CombatEntity>();
        _events = new List<ResolvedEvent>();
        var occupied = new HashSet<Cell>();
        foreach (var entity in entities)
        {
            if (!_entities.TryAdd(entity.Id, entity))
                throw new ArgumentException($"Duplicate entity ID {entity.Id}.", nameof(entities));
            if (entity.ActionInterval < 1 || entity.NextActAt < 0)
                throw new ArgumentException($"Entity {entity.Id} has invalid initiative values.", nameof(entities));
            if (entity.ActionPoints < 0 || entity.ActionPoints > 2)
                throw new ArgumentException($"Entity {entity.Id} has invalid action points.", nameof(entities));
            if (entity.ReactionCharges is < 0 or > 1)
                throw new ArgumentException($"Entity {entity.Id} has invalid reaction charges.", nameof(entities));
            if (!entity.Flags.Spatial) continue;
            foreach (var cell in entity.Footprint.OccupiedCells(entity.Anchor))
            {
                if (!Map.Contains(cell) || Map.GetTerrain(cell).Blocked)
                    throw new ArgumentException($"Entity {entity.Id} occupies invalid cell {cell}.", nameof(entities));
                if (!occupied.Add(cell))
                    throw new ArgumentException($"Entity {entity.Id} overlaps another entity at {cell}.", nameof(entities));
            }
        }
        if (activeEntityId is not null && !_entities.ContainsKey(activeEntityId.Value))
            throw new ArgumentException("Active entity ID is absent from state.", nameof(activeEntityId));
        ActiveEntityId = activeEntityId;
    }

    public IReadOnlyDictionary<EntityId, CombatEntity> Entities => _entities;
    public IReadOnlyList<ResolvedEvent> Events => _events;
    public BattleMap Map { get; }
    public CombatRules Rules { get; }
    public EntityId? ActiveEntityId { get; private set; }

    public CombatState Clone()
    {
        var clone = new CombatState(Map, Rules, _entities.Values, ActiveEntityId);
        clone._events.AddRange(_events);
        return clone;
    }

    internal CombatEntity Require(EntityId id) =>
        _entities.TryGetValue(id, out var entity)
            ? entity
            : throw new CommandRejectedException($"Unknown entity ID {id}.");

    internal ResolvedEvent Apply(CombatEvent payload)
    {
        switch (payload)
        {
            case EntityMovedEvent moved:
                _entities[moved.EntityId] = Require(moved.EntityId) with { Anchor = moved.To };
                break;
            case EntityDisplacedEvent displaced:
                _entities[displaced.TargetId] = Require(displaced.TargetId) with { Anchor = displaced.To };
                break;
            case IntegrityDamagedEvent damaged:
                var target = Require(damaged.TargetId);
                _entities[damaged.TargetId] = target with
                {
                    Integrity = new IntegrityPool(damaged.After, target.Integrity.Maximum)
                };
                break;
            case GuardGrantedEvent granted:
                var guardTarget = Require(granted.TargetId);
                _entities[granted.TargetId] = guardTarget with { Guard = guardTarget.Guard.Grant(granted.Amount) };
                break;
            case GuardDamagedEvent guardDamaged:
                var guardedTarget = Require(guardDamaged.TargetId);
                _entities[guardDamaged.TargetId] = guardedTarget with { Guard = guardedTarget.Guard.Absorb(guardDamaged.Amount) };
                break;
            case DeploymentModeChangedEvent deployment:
                var pilot = Require(deployment.PilotId);
                var mech = Require(deployment.MechId);
                _entities[deployment.PilotId] = pilot with
                {
                    Flags = deployment.Mode == DeploymentMode.Docked ? EntityFlags.Docked : EntityFlags.Active
                };
                _entities[deployment.MechId] = mech with { Flags = EntityFlags.Active };
                break;
            case ActionPointsSpentEvent spent:
                var spender = Require(spent.EntityId);
                _entities[spent.EntityId] = spender with { ActionPoints = spent.After };
                break;
            case ActionPointsRefreshedEvent refreshed:
                var refreshedEntity = Require(refreshed.EntityId);
                _entities[refreshed.EntityId] = refreshedEntity with
                {
                    ActionPoints = refreshed.After,
                    NextActAt = refreshed.NewNextActAt
                };
                ActiveEntityId = refreshed.EntityId;
                break;
            case ActivationEndedEvent ended:
                if (ActiveEntityId != ended.EntityId)
                    throw new InvalidOperationException("Cannot end an inactive entity's activation.");
                ActiveEntityId = null;
                break;
            case ConditionAppliedEvent applied:
                var conditioned = Require(applied.TargetId);
                _entities[applied.TargetId] = conditioned with
                {
                    Conditions = conditioned.Conditions.Apply(
                        new AppliedCondition(applied.Kind, applied.Duration, applied.SourceId))
                };
                break;
            case ConditionsAdvancedEvent advanced:
                var advancedEntity = Require(advanced.EntityId);
                _entities[advanced.EntityId] = advancedEntity with { Conditions = advanced.After };
                break;
            case ReactionChargeRefreshedEvent refreshedReaction:
                var reactionOwner = Require(refreshedReaction.EntityId);
                _entities[refreshedReaction.EntityId] = reactionOwner with { ReactionCharges = refreshedReaction.After };
                break;
            case ReactionChargeSpentEvent spentReaction:
                var reactor = Require(spentReaction.EntityId);
                _entities[spentReaction.EntityId] = reactor with { ReactionCharges = spentReaction.After };
                break;
            case ReactionTriggeredEvent:
            case DamageRedirectedEvent:
                break;
            default:
                throw new InvalidOperationException($"Unsupported event type {payload.GetType().Name}.");
        }

        var resolved = new ResolvedEvent(_events.Count, payload, DeterministicHash());
        _events.Add(resolved);
        return resolved;
    }

    public string DeterministicHash()
    {
        using var stream = new MemoryStream();
        using (var writer = new BinaryWriter(stream, System.Text.Encoding.UTF8, true))
        {
            Map.WriteDeterministic(writer);
            writer.Write((int)Rules.MovementTopology);
            writer.Write(Rules.MaximumClimb);
            writer.Write(ActiveEntityId is not null);
            if (ActiveEntityId is not null) writer.Write(ActiveEntityId.Value.Value);
            foreach (var pair in _entities)
            {
                var entity = pair.Value;
                writer.Write(entity.Id.Value);
                writer.Write(entity.Anchor.X);
                writer.Write(entity.Anchor.Y);
                writer.Write(entity.Footprint.Width);
                writer.Write(entity.Footprint.Height);
                writer.Write(entity.Flags.Spatial);
                writer.Write(entity.Flags.Selectable);
                writer.Write(entity.Flags.Targetable);
                writer.Write(entity.Flags.HasInitiativeSlot);
                writer.Write(entity.Integrity.Current);
                writer.Write(entity.Integrity.Maximum);
                writer.Write(entity.NextActAt);
                writer.Write(entity.ActionInterval);
                writer.Write((int)entity.Faction);
                writer.Write(entity.ActionPoints);
                writer.Write(entity.ReactionCharges);
                writer.Write(entity.Guard.Current);
                writer.Write((int)entity.Mass);
                writer.Write(entity.Conditions.Items.Count);
                foreach (var condition in entity.Conditions.Items)
                {
                    writer.Write((int)condition.Kind);
                    writer.Write(condition.RemainingActivations);
                    writer.Write(condition.SourceId.Value);
                }
            }
        }

        using var sha256 = SHA256.Create();
        return ToUpperHex(sha256.ComputeHash(stream.ToArray()));
    }

    private static string ToUpperHex(byte[] bytes)
    {
        const string digits = "0123456789ABCDEF";
        var characters = new char[bytes.Length * 2];
        for (var index = 0; index < bytes.Length; index++)
        {
            characters[index * 2] = digits[bytes[index] >> 4];
            characters[index * 2 + 1] = digits[bytes[index] & 0x0F];
        }

        return new string(characters);
    }

}
