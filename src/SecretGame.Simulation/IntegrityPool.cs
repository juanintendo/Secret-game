namespace SecretGame.Simulation;

public readonly record struct IntegrityPool
{
    public int Current { get; }
    public int Maximum { get; }

    public IntegrityPool(int current, int maximum)
    {
        if (maximum < 1) throw new ArgumentOutOfRangeException(nameof(maximum));
        if (current < 0 || current > maximum) throw new ArgumentOutOfRangeException(nameof(current));
        Current = current;
        Maximum = maximum;
    }

    public IntegrityPool ApplyDamage(int amount)
    {
        if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));
        return new IntegrityPool(Math.Max(0, Current - amount), Maximum);
    }
}
