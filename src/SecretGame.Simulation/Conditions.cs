namespace SecretGame.Simulation;

public enum ConditionKind
{
    Exposed,
    Prone,
    Staggered,
    Pinned,
    Shocked,
    Hacked,
    Marked,
    Hasted,
    Slowed,
    Guarded,
    Surrounded,
    Isolated,
    Elevated
}

public readonly record struct AppliedCondition(
    ConditionKind Kind,
    int RemainingActivations,
    EntityId SourceId);

public sealed class ConditionSet : IEquatable<ConditionSet>
{
    private readonly AppliedCondition[] _conditions;

    public static ConditionSet Empty { get; } = new(Array.Empty<AppliedCondition>());

    public ConditionSet(IEnumerable<AppliedCondition> conditions)
    {
        _conditions = conditions
            .OrderBy(condition => condition.Kind)
            .ThenBy(condition => condition.SourceId)
            .ToArray();
        if (_conditions.Any(condition => condition.RemainingActivations < 1))
            throw new ArgumentOutOfRangeException(nameof(conditions));
        if (_conditions.GroupBy(condition => condition.Kind).Any(group => group.Count() > 1))
            throw new ArgumentException("An applied condition kind may appear only once.", nameof(conditions));
    }

    public IReadOnlyList<AppliedCondition> Items => _conditions;

    public ConditionSet Apply(AppliedCondition condition)
    {
        if (ConditionRules.IsDerived(condition.Kind))
            throw new ArgumentException("Derived conditions cannot be stored.", nameof(condition));
        return new ConditionSet(_conditions
            .Where(existing => existing.Kind != condition.Kind)
            .Append(condition));
    }

    public ConditionSet AdvanceActivation() => new(_conditions
        .Where(condition => condition.RemainingActivations > 1)
        .Select(condition => condition with
        {
            RemainingActivations = condition.RemainingActivations - 1
        }));

    public bool Equals(ConditionSet? other) =>
        other is not null && _conditions.SequenceEqual(other._conditions);

    public override bool Equals(object? value) => Equals(value as ConditionSet);

    public override int GetHashCode()
    {
        var hash = new HashCode();
        foreach (var condition in _conditions) hash.Add(condition);
        return hash.ToHashCode();
    }
}

public static class ConditionRules
{
    public static bool IsDerived(ConditionKind kind) => kind is
        ConditionKind.Surrounded or ConditionKind.Isolated or ConditionKind.Elevated;
}

public readonly record struct ConditionSignal(EntityId EntityId, ConditionKind Kind) : IComparable<ConditionSignal>
{
    public int CompareTo(ConditionSignal other)
    {
        var entityOrder = EntityId.CompareTo(other.EntityId);
        return entityOrder != 0 ? entityOrder : Kind.CompareTo(other.Kind);
    }
}
