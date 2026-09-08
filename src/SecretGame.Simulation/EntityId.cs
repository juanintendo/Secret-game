namespace SecretGame.Simulation;

public readonly record struct EntityId(ulong Value) : IComparable<EntityId>
{
    public int CompareTo(EntityId other) => Value.CompareTo(other.Value);
    public override string ToString() => Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
}
