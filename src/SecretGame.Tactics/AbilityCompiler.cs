using SecretGame.Content;
using SecretGame.Simulation;

namespace SecretGame.Tactics;

public sealed class AbilityCompiler
{
    public EffectStackCommand Compile(
        AbilityDefinition ability,
        EntityId sourceId,
        EntityId targetId,
        CompassDirection displacementDirection = CompassDirection.North)
    {
        ArgumentNullException.ThrowIfNull(ability);
        var effects = ability.Effects.Select(effect => CompileEffect(effect, targetId, displacementDirection)).ToArray();
        return new EffectStackCommand(sourceId, ability.CostAp, effects);
    }

    private static CombatEffect CompileEffect(
        EffectDefinition effect,
        EntityId targetId,
        CompassDirection displacementDirection) => effect.Type switch
    {
        EffectType.Damage => new DamageEffect(targetId, RequiredPositive(effect.Amount, "damage amount")),
        EffectType.ApplyStatus => new ApplyConditionEffect(
            targetId,
            ParseCondition(effect.Status),
            RequiredPositive(effect.Duration, "status duration")),
        EffectType.Displace => new DisplaceEffect(
            targetId,
            displacementDirection,
            RequiredPositive(effect.Distance, "displacement distance"),
            1),
        _ => throw new NotSupportedException($"Effect {effect.Type} is authored but not executable in milestone 006.")
    };

    private static int RequiredPositive(int? value, string field) =>
        value is > 0 ? value.Value : throw new InvalidOperationException($"Missing or invalid {field}.");

    private static ConditionKind ParseCondition(string? value) => value switch
    {
        "exposed" => ConditionKind.Exposed,
        "prone" => ConditionKind.Prone,
        "staggered" => ConditionKind.Staggered,
        "pinned" => ConditionKind.Pinned,
        "shocked" => ConditionKind.Shocked,
        "hacked" => ConditionKind.Hacked,
        "marked" => ConditionKind.Marked,
        "hasted" => ConditionKind.Hasted,
        "slowed" => ConditionKind.Slowed,
        "guarded" => ConditionKind.Guarded,
        _ => throw new InvalidOperationException($"Unknown or non-authorable status {value ?? "<null>"}.")
    };
}
