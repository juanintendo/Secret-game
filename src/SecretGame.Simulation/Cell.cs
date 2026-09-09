namespace SecretGame.Simulation;

public readonly record struct Cell(int X, int Y) : IComparable<Cell>
{
    public int CompareTo(Cell other)
    {
        var yOrder = Y.CompareTo(other.Y);
        return yOrder != 0 ? yOrder : X.CompareTo(other.X);
    }
}
