namespace SecretGame.Simulation;

public sealed class ConditionEvaluator
{
    public IReadOnlyList<ConditionSignal> Evaluate(CombatState state)
    {
        var signals = new SortedSet<ConditionSignal>();
        foreach (var entity in state.Entities.Values)
        {
            foreach (var condition in entity.Conditions.Items)
                signals.Add(new ConditionSignal(entity.Id, condition.Kind));

            if (!entity.Flags.Spatial) continue;
            if (entity.Footprint.OccupiedCells(entity.Anchor)
                .Any(cell => state.Map.GetTerrain(cell).Elevation > 0))
                signals.Add(new ConditionSignal(entity.Id, ConditionKind.Elevated));

            var neighbours = state.Entities.Values
                .Where(other => other.Id != entity.Id && other.Flags.Spatial && AreAdjacent(entity, other))
                .ToArray();
            if (!neighbours.Any(other => other.Faction == entity.Faction))
                signals.Add(new ConditionSignal(entity.Id, ConditionKind.Isolated));
            if (neighbours.Count(other => other.Faction != entity.Faction) >= 2)
                signals.Add(new ConditionSignal(entity.Id, ConditionKind.Surrounded));
        }
        return signals.ToArray();
    }

    private static bool AreAdjacent(CombatEntity first, CombatEntity second) =>
        first.Footprint.OccupiedCells(first.Anchor).Any(firstCell =>
            second.Footprint.OccupiedCells(second.Anchor).Any(secondCell =>
                Math.Max(Math.Abs(firstCell.X - secondCell.X), Math.Abs(firstCell.Y - secondCell.Y)) == 1));
}
