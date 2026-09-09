namespace SecretGame.Simulation;

public readonly record struct InitiativeSlot(EntityId EntityId, int ActsAt);

public sealed class InitiativeTimeline
{
    public IReadOnlyList<InitiativeSlot> Query(CombatState state, int count)
    {
        if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
        var candidates = state.Entities.Values
            .Where(entity => entity.Flags.HasInitiativeSlot)
            .ToDictionary(entity => entity.Id, entity => new Cursor(entity.NextActAt, entity.ActionInterval));
        if (candidates.Values.Any(cursor => cursor.Interval < 1))
            throw new InvalidOperationException("Action interval must be positive.");

        var result = new List<InitiativeSlot>(count);
        for (var index = 0; index < count && candidates.Count > 0; index++)
        {
            var next = candidates
                .OrderBy(pair => pair.Value.ActsAt)
                .ThenBy(pair => pair.Key)
                .First();
            result.Add(new InitiativeSlot(next.Key, next.Value.ActsAt));
            candidates[next.Key] = next.Value with { ActsAt = next.Value.ActsAt + next.Value.Interval };
        }
        return result;
    }

    private readonly record struct Cursor(int ActsAt, int Interval);
}
