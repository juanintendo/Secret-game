namespace SecretGame.Simulation;

public readonly record struct Footprint
{
    public int Width { get; }
    public int Height { get; }

    public Footprint(int width, int height)
    {
        if (width < 1) throw new ArgumentOutOfRangeException(nameof(width));
        if (height < 1) throw new ArgumentOutOfRangeException(nameof(height));
        Width = width;
        Height = height;
    }

    public IEnumerable<Cell> OccupiedCells(Cell anchor)
    {
        for (var y = 0; y < Height; y++)
        for (var x = 0; x < Width; x++)
            yield return new Cell(anchor.X + x, anchor.Y + y);
    }
}
