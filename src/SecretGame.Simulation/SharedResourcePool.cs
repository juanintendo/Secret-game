namespace SecretGame.Simulation;

public readonly record struct ResourceId(ulong Value) : IComparable<ResourceId>
{
    public int CompareTo(ResourceId other) => Value.CompareTo(other.Value);
    public override string ToString() => Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
}

public sealed record SharedResourcePool
{
    public SharedResourcePool(
        ResourceId id,
        EntityId ownerId,
        EntityId partnerId,
        string name,
        int current,
        int maximum)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Resource name is required.", nameof(name));
        if (maximum < 1) throw new ArgumentOutOfRangeException(nameof(maximum));
        if (current < 0 || current > maximum) throw new ArgumentOutOfRangeException(nameof(current));
        Id = id;
        OwnerId = ownerId;
        PartnerId = partnerId;
        Name = name;
        Current = current;
        Maximum = maximum;
    }

    public ResourceId Id { get; }
    public EntityId OwnerId { get; }
    public EntityId PartnerId { get; }
    public string Name { get; }
    public int Current { get; }
    public int Maximum { get; }

    public SharedResourcePool WithCurrent(int current) => new(Id, OwnerId, PartnerId, Name, current, Maximum);
}

public enum ResourceChangeReason
{
    Recharge,
    Deployment,
    Ability,
    RemoteDirective
}
