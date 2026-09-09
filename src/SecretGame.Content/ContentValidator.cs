using System.Text.RegularExpressions;

namespace SecretGame.Content;

public sealed record ValidationReport(IReadOnlyList<string> Errors, IReadOnlyList<string> TwoPlusTwoPairings)
{
    public bool IsValid => Errors.Count == 0;
}

public sealed class ContentValidator
{
    private static readonly Regex ContentIdPattern = new("^[a-z0-9]+(?:[.-][a-z0-9]+)*$", RegexOptions.CultureInvariant);
    private static readonly HashSet<string> BuiltInReactionResponses = new(StringComparer.Ordinal)
    {
        "core.guard"
    };
    private static readonly IReadOnlyDictionary<string, string[]> MotionArchetypes =
        new Dictionary<string, string[]>(StringComparer.Ordinal)
        {
            ["human"] = new[] { "human-strike", "human-feint", "human-shot" },
            ["cyborg"] = new[] { "cyborg-intercept", "mech-salvo", "mech-stance" },
            ["synthetic"] = new[] { "syn-cast", "syn-hack", "syn-conduct" }
        };

    public ValidationReport Validate(ContentCatalog catalog)
    {
        var errors = new List<string>();
        var ids = new HashSet<string>(StringComparer.Ordinal);
        foreach (var id in catalog.Abilities.Select(item => item.Id)
            .Concat(catalog.Reactions.Select(item => item.Id))
            .Concat(catalog.Gear.Select(item => item.Id))
            .Concat(catalog.Weapons.Select(item => item.Id)))
        {
            if (!ContentIdPattern.IsMatch(id)) errors.Add($"Invalid content ID: {id}");
            if (!ids.Add(id)) errors.Add($"Duplicate content ID: {id}");
        }

        if (catalog.Abilities.Count != 9) errors.Add("Spike catalog must contain exactly nine abilities.");
        if (catalog.Reactions.Count != 3) errors.Add("Spike catalog must contain exactly three reactions.");
        if (catalog.Abilities.Select(ability => ability.MotionArchetype).Distinct(StringComparer.Ordinal).Count() > 8)
            errors.Add("Representative abilities exceed the eight-archetype animation budget.");
        if (Enum.GetValues<GearSlotIndex>().Length != 4) errors.Add("Gear slot index enum must contain exactly four values.");
        if (catalog.Weapons.Count != 1) errors.Add("Cyborg spike loadout must contain exactly one weapon.");

        foreach (var ability in catalog.Abilities)
        {
            if (ability.Version < 1) errors.Add($"{ability.Id} has invalid version.");
            if (ability.CostAp is < 1 or > 2) errors.Add($"{ability.Id} has invalid AP cost.");
            if (!MotionArchetypes.TryGetValue(ability.Owner, out var approved)
                || !approved.Contains(ability.MotionArchetype, StringComparer.Ordinal))
                errors.Add($"{ability.Id} references unapproved motion archetype {ability.MotionArchetype}.");
            if (ability.Effects.Any(effect => effect.Type == EffectType.Displace && effect.CollisionRule != "depth0"))
                errors.Add($"{ability.Id} has a displacement without collisionRule depth0.");
            if (ability.Effects.Any(effect => effect.Type is EffectType.Displace or EffectType.ApplyStatus)
                && (ability.BossDegradation is null || ability.BossDegradation.Length == 0))
                errors.Add($"{ability.Id} controls movement or status but lacks boss degradation.");
        }

        foreach (var reaction in catalog.Reactions)
        {
            if (reaction.PerEventLimit != 1) errors.Add($"{reaction.Id} must be limited to once per event.");
            if (!BuiltInReactionResponses.Contains(reaction.ResponseAbilityId)
                && !catalog.Abilities.Any(ability => ability.Id == reaction.ResponseAbilityId))
                errors.Add($"{reaction.Id} references unknown response ability.");
        }

        var pairings = EnumerateTwoPlusTwo(catalog.Gear);
        return new ValidationReport(errors, pairings);
    }

    public IReadOnlyList<string> ValidateLoadout(
        IReadOnlyList<GearDefinition> gear,
        WeaponDefinition weapon)
    {
        var errors = new List<string>();
        if (gear.Count != 4) errors.Add("Loadout requires exactly four gear records.");
        if (gear.Select(item => item.SlotIndex).Distinct().Count() != 4)
            errors.Add("Loadout requires four distinct gear slot indices.");
        if (gear.Any(item => item.Owner != weapon.Owner))
            errors.Add("Gear and weapon owners must match.");
        return errors;
    }

    private static IReadOnlyList<string> EnumerateTwoPlusTwo(IReadOnlyList<GearDefinition> gear)
    {
        var sets = gear.Where(item => item.SetId is not null)
            .GroupBy(item => item.SetId!, StringComparer.Ordinal)
            .Where(group => group.Count() >= 2)
            .OrderBy(group => group.Key, StringComparer.Ordinal)
            .ToArray();
        var pairings = new List<string>();
        for (var first = 0; first < sets.Length; first++)
        for (var second = first + 1; second < sets.Length; second++)
            pairings.Add($"{sets[first].Key}+{sets[second].Key}");
        return pairings;
    }

}
