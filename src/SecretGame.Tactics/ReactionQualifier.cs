using SecretGame.Content;
using SecretGame.Simulation;

namespace SecretGame.Tactics;

public sealed record ReactionProfile(
    EntityId EntityId,
    IReadOnlyList<string> ReactionIds,
    int BandMinimum = 0,
    int BandMaximum = 0);

public sealed class ReactionQualifier
{
    private const string GuardId = "reaction.guard";
    private const string InterceptId = "reaction.bulwark-intercept";
    private const string ReactionShotId = "reaction.gunslinger-shot";

    private readonly ContentCatalog _catalog;
    private readonly AbilityCompiler _compiler = new();
    private readonly CombatResolver _resolver = new();

    public ReactionQualifier(ContentCatalog catalog) => _catalog = catalog;

    public EffectStackCommand Qualify(
        CombatState state,
        EffectStackCommand command,
        IReadOnlyList<ReactionProfile> profiles)
    {
        if (command.Reactions is { Count: > 0 })
            throw new InvalidOperationException("Reaction qualification requires a reaction-free command.");

        var reactions = new List<ReactionInvocation>();
        var speculative = state.Clone();
        _resolver.Resolve(speculative, command with { Reactions = null });

        for (var index = 0; index < command.Effects.Count; index++)
        {
            if (command.Effects[index] is DamageEffect damage)
                QualifyDefense(state, damage, index, profiles, reactions);
            if (command.Effects[index] is DisplaceEffect displacement)
                QualifyReactionShots(state, speculative, displacement, index, profiles, reactions);
        }

        return command with { Reactions = reactions };
    }

    private void QualifyDefense(
        CombatState state,
        DamageEffect damage,
        int effectIndex,
        IReadOnlyList<ReactionProfile> profiles,
        List<ReactionInvocation> reactions)
    {
        var target = Get(state, damage.TargetId);
        var interceptor = profiles
            .Where(profile => profile.ReactionIds.Contains(InterceptId, StringComparer.Ordinal))
            .Select(profile => (Profile: profile, Entity: Get(state, profile.EntityId)))
            .Where(candidate => candidate.Entity.Id != target.Id
                && candidate.Entity.Faction == target.Faction
                && candidate.Entity.ReactionCharges > 0
                && Adjacent(candidate.Entity, target))
            .OrderBy(candidate => candidate.Entity.Id)
            .FirstOrDefault();

        if (interceptor.Entity is not null)
        {
            var ability = _catalog.Abilities.Single(item => item.Id == "cyborg.bulwark.intercept");
            var response = _compiler.Compile(ability, interceptor.Entity.Id, interceptor.Entity.Id).Effects;
            reactions.Add(new ReactionInvocation(
                interceptor.Entity.Id,
                effectIndex,
                InterceptId,
                response,
                ReactionTiming.Before,
                interceptor.Entity.Id));
            return;
        }

        var guard = profiles.FirstOrDefault(profile => profile.EntityId == target.Id
            && profile.ReactionIds.Contains(GuardId, StringComparer.Ordinal));
        if (guard is not null && target.ReactionCharges > 0 && target.Guard.Current > 0
            && target.Conditions.Items.Any(item => item.Kind == ConditionKind.Guarded))
            reactions.Add(new ReactionInvocation(
                target.Id,
                effectIndex,
                GuardId,
                Array.Empty<CombatEffect>(),
                ReactionTiming.Before));
    }

    private void QualifyReactionShots(
        CombatState before,
        CombatState after,
        DisplaceEffect displacement,
        int effectIndex,
        IReadOnlyList<ReactionProfile> profiles,
        List<ReactionInvocation> reactions)
    {
        var targetBefore = Get(before, displacement.TargetId);
        var targetAfter = Get(after, displacement.TargetId);
        foreach (var candidate in profiles
                     .Where(profile => profile.ReactionIds.Contains(ReactionShotId, StringComparer.Ordinal))
                     .Select(profile => (Profile: profile, Before: Get(before, profile.EntityId), After: Get(after, profile.EntityId)))
                     .Where(candidate => candidate.Before.Faction != targetBefore.Faction
                         && candidate.Before.ReactionCharges > 0
                         && candidate.Profile.BandMinimum >= 0
                         && candidate.Profile.BandMaximum >= candidate.Profile.BandMinimum)
                     .Where(candidate => !InBand(candidate.Before.Anchor, targetBefore.Anchor, candidate.Profile)
                         && InBand(candidate.After.Anchor, targetAfter.Anchor, candidate.Profile)
                         && new LineOfSight().CanSee(after, candidate.After, targetAfter))
                     .OrderBy(candidate => candidate.Before.Id)
                     .Take(2))
        {
            var ability = _catalog.Abilities.Single(item => item.Id == "human.gunslinger.reaction-shot");
            var response = _compiler.Compile(ability, candidate.Before.Id, targetAfter.Id).Effects;
            reactions.Add(new ReactionInvocation(
                candidate.Before.Id,
                effectIndex,
                ReactionShotId,
                response,
                ReactionTiming.After));
        }
    }

    private static bool InBand(Cell first, Cell second, ReactionProfile profile)
    {
        var distance = Math.Max(Math.Abs(first.X - second.X), Math.Abs(first.Y - second.Y));
        return distance >= profile.BandMinimum && distance <= profile.BandMaximum;
    }

    private static bool Adjacent(CombatEntity first, CombatEntity second) =>
        first.Footprint.OccupiedCells(first.Anchor).Any(firstCell =>
            second.Footprint.OccupiedCells(second.Anchor).Any(secondCell =>
                Math.Max(Math.Abs(firstCell.X - secondCell.X), Math.Abs(firstCell.Y - secondCell.Y)) == 1));

    private static CombatEntity Get(CombatState state, EntityId id) =>
        state.Entities.TryGetValue(id, out var entity)
            ? entity
            : throw new InvalidOperationException($"Reaction profile references absent entity {id}.");
}
