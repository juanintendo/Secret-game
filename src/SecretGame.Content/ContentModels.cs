namespace SecretGame.Content;

public enum EffectType
{
    Damage,
    ApplyStatus,
    RemoveStatus,
    Displace,
    Move,
    ModifyInitiative,
    GrantGuard,
    SpendResource,
    EmitTag,
    ConsumeTag,
    Link,
    Conditional
}

public enum GearSlotIndex
{
    Slot0,
    Slot1,
    Slot2,
    Slot3
}

public sealed record EffectDefinition
{
    public required EffectType Type { get; init; }
    public int? Amount { get; init; }
    public string? Status { get; init; }
    public int? Duration { get; init; }
    public int? Distance { get; init; }
    public string? CollisionRule { get; init; }
}

public sealed record AbilityDefinition
{
    public required string Id { get; init; }
    public required int Version { get; init; }
    public required string Owner { get; init; }
    public required string Specialty { get; init; }
    public required int CostAp { get; init; }
    public required string MotionArchetype { get; init; }
    public required EffectDefinition[] Effects { get; init; }
    public EffectDefinition[]? BossDegradation { get; init; }
}

public sealed record ReactionDefinition
{
    public required string Id { get; init; }
    public required int Version { get; init; }
    public required string Owner { get; init; }
    public required string Trigger { get; init; }
    public required string ResponseAbilityId { get; init; }
    public required int PerEventLimit { get; init; }
}

public sealed record GearDefinition
{
    public required string Id { get; init; }
    public required int Version { get; init; }
    public required string Owner { get; init; }
    public required GearSlotIndex SlotIndex { get; init; }
    public string? SetId { get; init; }
    public string? Role { get; init; }
}

public sealed record WeaponDefinition
{
    public required string Id { get; init; }
    public required int Version { get; init; }
    public required string Owner { get; init; }
    public required string WeaponClass { get; init; }
}

public sealed record ContentCatalog(
    IReadOnlyList<AbilityDefinition> Abilities,
    IReadOnlyList<ReactionDefinition> Reactions,
    IReadOnlyList<GearDefinition> Gear,
    IReadOnlyList<WeaponDefinition> Weapons);
