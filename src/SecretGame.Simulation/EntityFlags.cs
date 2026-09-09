namespace SecretGame.Simulation;

public readonly record struct EntityFlags(
    bool Spatial,
    bool Selectable,
    bool Targetable,
    bool HasInitiativeSlot)
{
    public static EntityFlags Active { get; } = new(true, true, true, true);
    public static EntityFlags Docked { get; } = new(false, false, false, false);
    public static EntityFlags RemoteControlled { get; } = new(true, true, true, false);
}
