namespace SecretGame.Simulation;

public sealed class CellPath : IEquatable<CellPath>
{
    private readonly Cell[] _cells;

    public CellPath(IEnumerable<Cell> cells) => _cells = cells.ToArray();

    public IReadOnlyList<Cell> Cells => _cells;
    public int StepCount => Math.Max(0, _cells.Length - 1);

    public bool Equals(CellPath? other) =>
        other is not null && _cells.SequenceEqual(other._cells);

    public override bool Equals(object? value) => Equals(value as CellPath);

    public override int GetHashCode()
    {
        var hash = new HashCode();
        foreach (var cell in _cells) hash.Add(cell);
        return hash.ToHashCode();
    }
}
