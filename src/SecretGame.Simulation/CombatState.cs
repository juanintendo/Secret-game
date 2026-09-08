using System.Security.Cryptography;

namespace SecretGame.Simulation;

public sealed class CombatState
{
    private readonly SortedDictionary<EntityId, CombatEntity> _entities;
    private readonly List<ResolvedEvent> _events;

    public CombatState(IEnumerable<CombatEntity> entities)
    {
        _entities = new SortedDictionary<EntityId, CombatEntity>();
        _events = new List<ResolvedEvent>();
        foreach (var entity in entities)
        {
            if (!_entities.TryAdd(entity.Id, entity))
                throw new ArgumentException($"Duplicate entity ID {entity.Id}.", nameof(entities));
        }
    }

    public IReadOnlyDictionary<EntityId, CombatEntity> Entities => _entities;
    public IReadOnlyList<ResolvedEvent> Events => _events;

    public CombatState Clone()
    {
        var clone = new CombatState(_entities.Values);
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
            case IntegrityDamagedEvent damaged:
                var target = Require(damaged.TargetId);
                _entities[damaged.TargetId] = target with
                {
                    Integrity = new IntegrityPool(damaged.After, target.Integrity.Maximum)
                };
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
            }
        }

        return Convert.ToHexString(SHA256.HashData(stream.ToArray()));
    }

}
