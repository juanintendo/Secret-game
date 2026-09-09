namespace SecretGame.Simulation;

public enum CoverLevel
{
    None,
    Half,
    Full
}

public readonly record struct TerrainTile(Cell Cell, int Elevation, bool Blocked);

public readonly record struct CoverEdge(Cell First, Cell Second, CoverLevel Level);

public sealed class BattleMap
{
    private readonly SortedDictionary<Cell, TerrainTile> _terrain;
    private readonly SortedDictionary<EdgeKey, CoverLevel> _cover;

    public BattleMap(
        int width,
        int height,
        IEnumerable<TerrainTile>? terrain = null,
        IEnumerable<CoverEdge>? cover = null)
    {
        if (width < 1) throw new ArgumentOutOfRangeException(nameof(width));
        if (height < 1) throw new ArgumentOutOfRangeException(nameof(height));
        Width = width;
        Height = height;
        _terrain = new SortedDictionary<Cell, TerrainTile>();
        _cover = new SortedDictionary<EdgeKey, CoverLevel>();

        foreach (var tile in terrain ?? Array.Empty<TerrainTile>())
        {
            if (!Contains(tile.Cell)) throw new ArgumentOutOfRangeException(nameof(terrain));
            _terrain[tile.Cell] = tile;
        }

        foreach (var edge in cover ?? Array.Empty<CoverEdge>())
        {
            if (!Contains(edge.First) || !Contains(edge.Second))
                throw new ArgumentOutOfRangeException(nameof(cover));
            var separation = Math.Abs(edge.First.X - edge.Second.X) + Math.Abs(edge.First.Y - edge.Second.Y);
            if (separation != 1) throw new ArgumentException("Cover must occupy one cardinal tile edge.", nameof(cover));
            _cover[new EdgeKey(edge.First, edge.Second)] = edge.Level;
        }
    }

    public int Width { get; }
    public int Height { get; }

    public bool Contains(Cell cell) =>
        cell.X >= 0 && cell.X < Width && cell.Y >= 0 && cell.Y < Height;

    public TerrainTile GetTerrain(Cell cell)
    {
        if (!Contains(cell)) throw new ArgumentOutOfRangeException(nameof(cell));
        return _terrain.TryGetValue(cell, out var tile) ? tile : new TerrainTile(cell, 0, false);
    }

    public CoverLevel GetCover(Cell first, Cell second) =>
        _cover.TryGetValue(new EdgeKey(first, second), out var level) ? level : CoverLevel.None;

    internal void WriteDeterministic(BinaryWriter writer)
    {
        writer.Write(Width);
        writer.Write(Height);
        writer.Write(_terrain.Count);
        foreach (var pair in _terrain)
        {
            writer.Write(pair.Key.X);
            writer.Write(pair.Key.Y);
            writer.Write(pair.Value.Elevation);
            writer.Write(pair.Value.Blocked);
        }

        writer.Write(_cover.Count);
        foreach (var pair in _cover)
        {
            writer.Write(pair.Key.First.X);
            writer.Write(pair.Key.First.Y);
            writer.Write(pair.Key.Second.X);
            writer.Write(pair.Key.Second.Y);
            writer.Write((int)pair.Value);
        }
    }

    private readonly record struct EdgeKey : IComparable<EdgeKey>
    {
        public EdgeKey(Cell first, Cell second)
        {
            if (first.CompareTo(second) <= 0) { First = first; Second = second; }
            else { First = second; Second = first; }
        }

        public Cell First { get; }
        public Cell Second { get; }

        public int CompareTo(EdgeKey other)
        {
            var firstOrder = First.CompareTo(other.First);
            return firstOrder != 0 ? firstOrder : Second.CompareTo(other.Second);
        }
    }
}
