namespace SecretGame.Simulation;

public readonly record struct GuardPool
{
    public static GuardPool Empty => new(0);

    public GuardPool(int current)
    {
        if (current < 0) throw new ArgumentOutOfRangeException(nameof(current));
        Current = current;
    }

    public int Current { get; }

    public GuardPool Grant(int amount)
    {
        if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));
        return new GuardPool(checked(Current + amount));
    }

    public GuardPool Absorb(int amount)
    {
        if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));
        return new GuardPool(Math.Max(0, Current - amount));
    }
}
