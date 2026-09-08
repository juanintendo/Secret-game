using System.Security.Cryptography;

namespace SecretGame.Simulation;

public sealed class CombatState
{
    private readonly SortedDictionary<EntityId, CombatEntity> _entities;

    public CombatState(IEnumerable<CombatEntity> entities)
    {
        _entities = new SortedDictionary<EntityId, CombatEntity>();
        foreach (var entity in entities)
        {
            if (!_entities.TryAdd(entity.Id, entity))
                throw new ArgumentException($"Duplicate entity ID {entity.Id}.", nameof(entities));
        }
    }

    public IReadOnlyDictionary<EntityId, CombatEntity> Entities => _entities;

    public void SetPilotMechMode(EntityId pilotId, EntityId mechId, DeploymentMode mode)
    {
        var pilot = Require(pilotId);
        var mech = Require(mechId);
        _entities[pilotId] = pilot with { Flags = mode == DeploymentMode.Docked ? EntityFlags.Docked : EntityFlags.Active };
        _entities[mechId] = mech with { Flags = EntityFlags.Active };
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

    private CombatEntity Require(EntityId id) =>
        _entities.TryGetValue(id, out var entity)
            ? entity
            : throw new KeyNotFoundException($"Unknown entity ID {id}.");
}
