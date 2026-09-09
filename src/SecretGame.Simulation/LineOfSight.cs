namespace SecretGame.Simulation;

public sealed class LineOfSight
{
    public bool CanSee(CombatState state, CombatEntity observer, CombatEntity target)
    {
        if (!observer.Flags.Spatial || !target.Flags.Spatial) return false;
        return observer.Footprint.OccupiedCells(observer.Anchor).Any(from =>
            target.Footprint.OccupiedCells(target.Anchor).Any(to => HasClearLine(state.Map, from, to)));
    }

    public bool HasClearLine(BattleMap map, Cell from, Cell to)
    {
        if (!map.Contains(from) || !map.Contains(to)) return false;
        var ceiling = Math.Max(map.GetTerrain(from).Elevation, map.GetTerrain(to).Elevation);
        var cells = Trace(from, to).ToArray();
        for (var index = 1; index < cells.Length - 1; index++)
        {
            var terrain = map.GetTerrain(cells[index]);
            if (terrain.Blocked || terrain.Elevation > ceiling) return false;
        }
        return true;
    }

    private static IEnumerable<Cell> Trace(Cell from, Cell to)
    {
        var x = from.X;
        var y = from.Y;
        var deltaX = Math.Abs(to.X - from.X);
        var stepX = from.X < to.X ? 1 : -1;
        var deltaY = -Math.Abs(to.Y - from.Y);
        var stepY = from.Y < to.Y ? 1 : -1;
        var error = deltaX + deltaY;

        while (true)
        {
            yield return new Cell(x, y);
            if (x == to.X && y == to.Y) yield break;
            var doubledError = error * 2;
            if (doubledError >= deltaY) { error += deltaY; x += stepX; }
            if (doubledError <= deltaX) { error += deltaX; y += stepY; }
        }
    }
}
