namespace SecretGame.Simulation;

public sealed class GridPathfinder
{
    private static readonly Cell[] CardinalOffsets =
    {
        new(0, -1), new(1, 0), new(0, 1), new(-1, 0)
    };

    private static readonly Cell[] DiagonalOffsets =
    {
        new(1, -1), new(1, 1), new(-1, 1), new(-1, -1)
    };

    public CellPath? FindPath(CombatState state, CombatEntity entity, Cell destination)
    {
        if (!CanOccupy(state, entity, destination)) return null;

        var frontier = new Queue<Cell>();
        var previous = new Dictionary<Cell, Cell?> { [entity.Anchor] = null };
        frontier.Enqueue(entity.Anchor);

        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();
            if (current == destination) return BuildPath(previous, current);

            foreach (var next in Neighbours(current, state.Rules.MovementTopology))
            {
                if (previous.ContainsKey(next)) continue;
                if (!CanTraverse(state, entity, current, next)) continue;
                previous[next] = current;
                frontier.Enqueue(next);
            }
        }

        return null;
    }

    private static bool CanTraverse(CombatState state, CombatEntity entity, Cell from, Cell to)
    {
        if (!CanOccupy(state, entity, to)) return false;
        if (!ElevationAllowed(state, entity, from, to)) return false;

        var deltaX = to.X - from.X;
        var deltaY = to.Y - from.Y;
        if (deltaX == 0 || deltaY == 0) return true;

        var horizontal = new Cell(from.X + deltaX, from.Y);
        var vertical = new Cell(from.X, from.Y + deltaY);
        return CanOccupy(state, entity, horizontal)
            && CanOccupy(state, entity, vertical)
            && ElevationAllowed(state, entity, from, horizontal)
            && ElevationAllowed(state, entity, from, vertical);
    }

    private static bool ElevationAllowed(CombatState state, CombatEntity entity, Cell from, Cell to)
    {
        for (var y = 0; y < entity.Footprint.Height; y++)
        for (var x = 0; x < entity.Footprint.Width; x++)
        {
            var fromCell = new Cell(from.X + x, from.Y + y);
            var toCell = new Cell(to.X + x, to.Y + y);
            var fromElevation = state.Map.GetTerrain(fromCell).Elevation;
            var toElevation = state.Map.GetTerrain(toCell).Elevation;
            if (Math.Abs(toElevation - fromElevation) > state.Rules.MaximumClimb) return false;
        }
        return true;
    }

    private static bool CanOccupy(CombatState state, CombatEntity entity, Cell anchor)
    {
        var candidate = entity.Footprint.OccupiedCells(anchor).ToArray();
        if (candidate.Any(cell => !state.Map.Contains(cell) || state.Map.GetTerrain(cell).Blocked)) return false;

        var cells = candidate.ToHashSet();
        return state.Entities.Values
            .Where(other => other.Id != entity.Id && other.Flags.Spatial)
            .All(other => !other.Footprint.OccupiedCells(other.Anchor).Any(cells.Contains));
    }

    private static IEnumerable<Cell> Neighbours(Cell cell, MovementTopology topology)
    {
        foreach (var offset in CardinalOffsets)
            yield return new Cell(cell.X + offset.X, cell.Y + offset.Y);
        if (topology != MovementTopology.EightConnected) yield break;
        foreach (var offset in DiagonalOffsets)
            yield return new Cell(cell.X + offset.X, cell.Y + offset.Y);
    }

    private static CellPath BuildPath(IReadOnlyDictionary<Cell, Cell?> previous, Cell destination)
    {
        var reversed = new List<Cell>();
        Cell? current = destination;
        while (current is not null)
        {
            reversed.Add(current.Value);
            current = previous[current.Value];
        }
        reversed.Reverse();
        return new CellPath(reversed);
    }
}
