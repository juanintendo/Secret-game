namespace SecretGame.Simulation;

public abstract record CombatEffect;

public sealed record DamageEffect(EntityId TargetId, int Amount, bool Piercing = false) : CombatEffect;

public sealed record GrantGuardEffect(EntityId TargetId, int Amount) : CombatEffect;

public sealed record SpendResourceEffect(ResourceId ResourceId, int Amount) : CombatEffect;

public sealed record ApplyConditionEffect(
    EntityId TargetId,
    ConditionKind Kind,
    int Duration) : CombatEffect;

public sealed record DisplaceEffect(
    EntityId TargetId,
    CompassDirection Direction,
    int Distance,
    int ImpactDamage) : CombatEffect;

public sealed record ReactionInvocation(
    EntityId ReactorId,
    int TriggerEffectIndex,
    string ReactionId,
    IReadOnlyList<CombatEffect> ResponseEffects,
    ReactionTiming Timing = ReactionTiming.After,
    EntityId? RedirectTargetId = null);

public enum ReactionTiming
{
    Before,
    After
}

public enum CompassDirection
{
    North,
    NorthEast,
    East,
    SouthEast,
    South,
    SouthWest,
    West,
    NorthWest
}

public static class CompassDirectionExtensions
{
    public static Cell Step(this CompassDirection direction) => direction switch
    {
        CompassDirection.North => new Cell(0, -1),
        CompassDirection.NorthEast => new Cell(1, -1),
        CompassDirection.East => new Cell(1, 0),
        CompassDirection.SouthEast => new Cell(1, 1),
        CompassDirection.South => new Cell(0, 1),
        CompassDirection.SouthWest => new Cell(-1, 1),
        CompassDirection.West => new Cell(-1, 0),
        CompassDirection.NorthWest => new Cell(-1, -1),
        _ => throw new ArgumentOutOfRangeException(nameof(direction))
    };
}
