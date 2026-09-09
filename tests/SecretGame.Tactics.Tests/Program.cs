using System;
using System.Linq;
using SecretGame.Content;
using SecretGame.Simulation;
using SecretGame.Tactics;

var tests = new (string Name, Action Run)[]
{
    ("compiler creates one AP-priced typed stack", CompilerCreatesStack),
    ("multi-effect ability spends AP once", StackSpendsOnce),
    ("effect stack forecast equals execution", StackForecastParity),
    ("wall collision stops and damages displaced target", WallCollision),
    ("entity collision damages both without cascading push", EntityCollisionDepthZero),
    ("2x2 displaced footprint stops before narrow obstruction", MultiCellDisplacement),
    ("effect application cap rejects ninth effect", EffectCap),
    ("reaction consumes charge and resolves at depth zero", ReactionResolves),
    ("reaction per-unit and per-effect caps reject atomically", ReactionCaps),
    ("text forecast exposes displacement stop and reaction", TextForecastExposesStack),
    ("Guard qualifier consumes declared Guard before Integrity", GuardQualifier),
    ("Intercept qualifier redirects adjacent ally damage", InterceptQualifier),
    ("Reaction Shot qualifier fires only on band entry", ReactionShotQualifier),
    ("Piercing bypasses Guard", PiercingBypassesGuard),
    ("mass tiers reduce displacement without changing footprint", MassReducesDisplacement),
    ("Anchored degrades displacement to Staggered", AnchoredDegradesControl),
    ("2x2 pathing reaches objective on four narrow map variants", FourNarrowMapVariants)
};
var failures = 0;
foreach (var test in tests)
{
    try { test.Run(); Console.WriteLine($"PASS {test.Name}"); }
    catch (Exception error) { failures++; Console.Error.WriteLine($"FAIL {test.Name}: {error.Message}"); }
}
Console.WriteLine($"{tests.Length - failures}/{tests.Length} tactics tests passed");
return failures == 0 ? 0 : 1;

static void CompilerCreatesStack()
{
    var ability = new ContentLoader().Load("content").Abilities.Single(item => item.Id == "human.dispatch.break-line");
    var stack = new AbilityCompiler().Compile(ability, new EntityId(1), new EntityId(2), CompassDirection.East);
    Assert(stack.CostAp == 1 && stack.Effects.Count == 2, "Compiler changed ability shape.");
    Assert(stack.Effects[0] is DamageEffect && stack.Effects[1] is DisplaceEffect, "Compiler changed effect order.");
}

static void StackSpendsOnce()
{
    var state = State();
    var result = new CombatResolver().Resolve(state, Stack());
    Assert(result.Events.Count(item => item.Payload is ActionPointsSpentEvent) == 1, "Stack spent AP more than once.");
    Assert(state.Entities[new EntityId(1)].ActionPoints == 1, "Stack charged the wrong AP amount.");
    Assert(state.Entities[new EntityId(2)].Integrity.Current == 7, "Damage effect did not apply.");
    Assert(state.Entities[new EntityId(2)].Conditions.Items.Single().Kind == ConditionKind.Marked, "Status effect did not apply.");
}

static void StackForecastParity()
{
    var state = State();
    var resolver = new CombatResolver();
    var predicted = new CombatForecast(resolver).Evaluate(state, Stack());
    var actual = resolver.Resolve(state, Stack());
    Assert(predicted.IsLegal, predicted.RejectionReason ?? "Forecast rejected.");
    Assert(ReplayVerifier.FindFirstDivergence(predicted.Events, actual.Events) is null, "Stack events diverged.");
    Assert(predicted.ResultingStateHash == actual.ResultingStateHash, "Stack hash diverged.");
}

static void WallCollision()
{
    var map = new BattleMap(6, 4, new[] { new TerrainTile(new Cell(4, 1), 0, true) });
    var state = State(map, Target(2, 3, 1));
    var stack = new EffectStackCommand(new EntityId(1), 1,
        new CombatEffect[] { new DisplaceEffect(new EntityId(2), CompassDirection.East, 2, 2) });
    var result = new CombatResolver().Resolve(state, stack);
    var displaced = (EntityDisplacedEvent)result.Events.Single(item => item.Payload is EntityDisplacedEvent).Payload;
    Assert(displaced.Stop == DisplacementStop.Blocked && displaced.To == new Cell(3, 1), "Wall stop was incorrect.");
    Assert(state.Entities[new EntityId(2)].Integrity.Current == 8, "Wall impact damage was incorrect.");
}

static void EntityCollisionDepthZero()
{
    var obstacle = Target(3, 4, 1);
    var state = State(new BattleMap(7, 4), Target(2, 3, 1), obstacle);
    var stack = new EffectStackCommand(new EntityId(1), 1,
        new CombatEffect[] { new DisplaceEffect(new EntityId(2), CompassDirection.East, 3, 2) });
    new CombatResolver().Resolve(state, stack);
    Assert(state.Entities[new EntityId(2)].Anchor == new Cell(3, 1), "Displaced target entered occupied cell.");
    Assert(state.Entities[new EntityId(3)].Anchor == new Cell(4, 1), "Collision cascaded into a second push.");
    Assert(state.Entities[new EntityId(2)].Integrity.Current == 8 && state.Entities[new EntityId(3)].Integrity.Current == 8,
        "Entity collision did not damage both participants.");
}

static void MultiCellDisplacement()
{
    var map = new BattleMap(7, 5, new[] { new TerrainTile(new Cell(5, 2), 0, true) });
    var mech = new CombatEntity(new EntityId(2), new Cell(3, 1), new Footprint(2, 2), EntityFlags.Active, new IntegrityPool(20, 20))
        with { Faction = Faction.Enemy };
    var state = State(map, mech);
    var stack = new EffectStackCommand(new EntityId(1), 1,
        new CombatEffect[] { new DisplaceEffect(mech.Id, CompassDirection.East, 2, 1) });
    new CombatResolver().Resolve(state, stack);
    Assert(state.Entities[mech.Id].Anchor == new Cell(3, 1), "2x2 footprint overlapped obstruction.");
}

static void EffectCap()
{
    var effects = Enumerable.Range(0, 9).Select(_ => (CombatEffect)new DamageEffect(new EntityId(2), 1)).ToArray();
    Throws<CommandRejectedException>(() => new CombatResolver().Resolve(State(), new EffectStackCommand(new EntityId(1), 1, effects)));
}

static void ReactionResolves()
{
    var reactor = new CombatEntity(new EntityId(3), new Cell(1, 2), new Footprint(1, 1), EntityFlags.Active, new IntegrityPool(10, 10));
    var state = State(new BattleMap(8, 5), Target(2, 3, 1), reactor);
    var command = new EffectStackCommand(new EntityId(1), 1,
        new CombatEffect[] { new DamageEffect(new EntityId(2), 2) },
        new[] { new ReactionInvocation(reactor.Id, 0, "reaction.test", new CombatEffect[] { new DamageEffect(new EntityId(2), 1) }) });
    var result = new CombatResolver().Resolve(state, command);
    Assert(state.Entities[reactor.Id].ReactionCharges == 0, "Reaction charge was not consumed.");
    Assert(state.Entities[new EntityId(2)].Integrity.Current == 7, "Reaction response did not resolve.");
    Assert(result.Events.Count(item => item.Payload is ReactionTriggeredEvent) == 1, "Reaction recursively triggered another reaction.");
}

static void ReactionCaps()
{
    var reactor = new CombatEntity(new EntityId(3), new Cell(1, 2), new Footprint(1, 1), EntityFlags.Active, new IntegrityPool(10, 10));
    var state = State(new BattleMap(8, 5), Target(2, 3, 1), reactor);
    var before = state.DeterministicHash();
    var duplicate = new[]
    {
        new ReactionInvocation(reactor.Id, 0, "reaction.a", new CombatEffect[] { new DamageEffect(new EntityId(2), 1) }),
        new ReactionInvocation(reactor.Id, 0, "reaction.b", new CombatEffect[] { new DamageEffect(new EntityId(2), 1) })
    };
    Throws<CommandRejectedException>(() => new CombatResolver().Resolve(state,
        new EffectStackCommand(new EntityId(1), 1, new CombatEffect[] { new DamageEffect(new EntityId(2), 1) }, duplicate)));
    Assert(state.DeterministicHash() == before && state.Events.Count == 0, "Rejected reaction stack partially mutated state.");
}

static void TextForecastExposesStack()
{
    var reactor = new CombatEntity(new EntityId(3), new Cell(1, 2), new Footprint(1, 1), EntityFlags.Active, new IntegrityPool(10, 10));
    var state = State(new BattleMap(8, 5), Target(2, 3, 1), reactor);
    var command = new EffectStackCommand(new EntityId(1), 1,
        new CombatEffect[] { new DisplaceEffect(new EntityId(2), CompassDirection.East, 1, 1) },
        new[] { new ReactionInvocation(reactor.Id, 0, "reaction.test", new CombatEffect[] { new DamageEffect(new EntityId(2), 1) }) });
    var forecast = new CombatForecast(new CombatResolver()).Evaluate(state, command);
    var text = new TextForecastRenderer().RenderForecast(forecast);
    Assert(text.Contains("DISPLACE", StringComparison.Ordinal) && text.Contains("Completed", StringComparison.Ordinal),
        "Forecast omitted displacement path or stop reason.");
    Assert(text.Contains("TRIGGER reaction.test", StringComparison.Ordinal), "Forecast omitted reaction trigger.");
}

static void GuardQualifier()
{
    var target = Target(2, 3, 1) with
    {
        Guard = new GuardPool(3),
        Conditions = new ConditionSet(new[] { new AppliedCondition(ConditionKind.Guarded, 1, new EntityId(2)) })
    };
    var state = State(new BattleMap(8, 5), target);
    var baseCommand = new EffectStackCommand(new EntityId(1), 1, new CombatEffect[] { new DamageEffect(target.Id, 4) });
    var qualified = Qualifier().Qualify(state, baseCommand,
        new[] { new ReactionProfile(target.Id, new[] { "reaction.guard" }) });
    var result = new CombatResolver().Resolve(state, qualified);
    Assert(result.Events.Any(item => item.Payload is ReactionTriggeredEvent trigger && trigger.ReactionId == "reaction.guard"),
        "Declared Guard did not trigger.");
    Assert(state.Entities[target.Id].Guard.Current == 0 && state.Entities[target.Id].Integrity.Current == 9,
        "Guard did not absorb damage before Integrity.");
}

static void InterceptQualifier()
{
    var attacker = new CombatEntity(new EntityId(101), new Cell(6, 1), new Footprint(1, 1), EntityFlags.Active, new IntegrityPool(10, 10))
        { Faction = Faction.Enemy };
    var ally = new CombatEntity(new EntityId(2), new Cell(3, 1), new Footprint(1, 1), EntityFlags.Active, new IntegrityPool(10, 10));
    var bulwark = new CombatEntity(new EntityId(3), new Cell(3, 2), new Footprint(1, 1), EntityFlags.Active, new IntegrityPool(10, 10));
    var state = new CombatState(new BattleMap(8, 5), CombatRules.SpikeDefault, new[] { attacker, ally, bulwark }, attacker.Id);
    var baseCommand = new EffectStackCommand(attacker.Id, 1, new CombatEffect[] { new DamageEffect(ally.Id, 3) });
    var qualified = Qualifier().Qualify(state, baseCommand,
        new[] { new ReactionProfile(bulwark.Id, new[] { "reaction.bulwark-intercept" }) });
    var result = new CombatResolver().Resolve(state, qualified);
    Assert(result.Events.Any(item => item.Payload is DamageRedirectedEvent redirect && redirect.RedirectedTargetId == bulwark.Id),
        "Intercept did not redirect damage.");
    Assert(state.Entities[ally.Id].Integrity.Current == 10, "Intercepted ally still took damage.");
    Assert(state.Entities[bulwark.Id].Guard.Current == 0 && state.Entities[bulwark.Id].Integrity.Current == 9,
        "Bulwark response did not grant then spend Guard before taking overflow.");
}

static void ReactionShotQualifier()
{
    var attacker = new CombatEntity(new EntityId(101), new Cell(7, 1), new Footprint(1, 1), EntityFlags.Active, new IntegrityPool(10, 10))
        { Faction = Faction.Enemy };
    var mover = new CombatEntity(new EntityId(102), new Cell(5, 1), new Footprint(1, 1), EntityFlags.Active, new IntegrityPool(10, 10))
        { Faction = Faction.Enemy };
    var gunslinger = new CombatEntity(new EntityId(3), new Cell(1, 1), new Footprint(1, 1), EntityFlags.Active, new IntegrityPool(10, 10));
    var state = new CombatState(new BattleMap(9, 5), CombatRules.SpikeDefault, new[] { attacker, mover, gunslinger }, attacker.Id);
    var baseCommand = new EffectStackCommand(attacker.Id, 1,
        new CombatEffect[] { new DisplaceEffect(mover.Id, CompassDirection.West, 1, 0) });
    var qualified = Qualifier().Qualify(state, baseCommand,
        new[] { new ReactionProfile(gunslinger.Id, new[] { "reaction.gunslinger-shot" }, 1, 3) });
    new CombatResolver().Resolve(state, qualified);
    Assert(state.Entities[mover.Id].Anchor == new Cell(4, 1), "Mover did not enter reaction band.");
    Assert(state.Entities[mover.Id].Integrity.Current == 8, "Reaction Shot did not apply authored damage.");
    Assert(state.Entities[gunslinger.Id].ReactionCharges == 0, "Reaction Shot did not consume charge.");
}

static void PiercingBypassesGuard()
{
    var target = Target(2, 3, 1) with { Guard = new GuardPool(4) };
    var state = State(new BattleMap(8, 5), target);
    new CombatResolver().Resolve(state,
        new EffectStackCommand(new EntityId(1), 1, new CombatEffect[] { new DamageEffect(target.Id, 3, true) }));
    Assert(state.Entities[target.Id].Guard.Current == 4 && state.Entities[target.Id].Integrity.Current == 7,
        "Piercing did not bypass Guard.");
}

static void MassReducesDisplacement()
{
    var light = Target(2, 2, 1);
    var heavy = Target(3, 2, 3) with { Mass = MassClass.Heavy };
    var state = State(new BattleMap(9, 6), light, heavy);
    var command = new EffectStackCommand(new EntityId(1), 1, new CombatEffect[]
    {
        new DisplaceEffect(light.Id, CompassDirection.East, 3, 0),
        new DisplaceEffect(heavy.Id, CompassDirection.East, 3, 0)
    });
    new CombatResolver().Resolve(state, command);
    Assert(state.Entities[light.Id].Anchor == new Cell(5, 1), "Light displacement distance changed.");
    Assert(state.Entities[heavy.Id].Anchor == new Cell(3, 3), "Heavy displacement did not reduce by two tiers.");
}

static void AnchoredDegradesControl()
{
    var anchored = Target(2, 3, 1) with { Mass = MassClass.Anchored };
    var state = State(new BattleMap(8, 5), anchored);
    new CombatResolver().Resolve(state, new EffectStackCommand(new EntityId(1), 1,
        new CombatEffect[] { new DisplaceEffect(anchored.Id, CompassDirection.East, 4, 1) }));
    Assert(state.Entities[anchored.Id].Anchor == new Cell(3, 1), "Anchored target moved.");
    Assert(state.Entities[anchored.Id].Conditions.Items.Any(item => item.Kind == ConditionKind.Staggered),
        "Anchored target did not receive degraded control effect.");
}

static void FourNarrowMapVariants()
{
    var variants = new[]
    {
        Enumerable.Range(0, 14).Where(y => y is not 5 and not 6).Select(y => new TerrainTile(new Cell(6, y), 0, true)),
        Enumerable.Range(0, 14).Where(x => x is not 5 and not 6).Select(x => new TerrainTile(new Cell(x, 6), 0, true)),
        new[] { new Cell(5, 5), new Cell(8, 8), new Cell(5, 9), new Cell(9, 5) }.Select(cell => new TerrainTile(cell, 0, true)),
        Enumerable.Range(0, 14).Where(y => y is not 2 and not 3).Select(y => new TerrainTile(new Cell(4, y), 0, true))
            .Concat(Enumerable.Range(0, 14).Where(y => y is not 9 and not 10).Select(y => new TerrainTile(new Cell(9, y), 0, true)))
    };
    for (var index = 0; index < variants.Length; index++)
    {
        var mech = new CombatEntity(new EntityId(2), new Cell(1, 1), new Footprint(2, 2), EntityFlags.Active, new IntegrityPool(20, 20))
            { Mass = MassClass.Heavy };
        var state = new CombatState(new BattleMap(14, 14, variants[index]), CombatRules.SpikeDefault, new[] { mech });
        var path = new GridPathfinder().FindPath(state, mech, new Cell(10, 10));
        Assert(path is not null && path.StepCount > 0 && path.StepCount < 100,
            $"2x2 mech failed narrow map variant {index + 1}.");
    }
}

static ReactionQualifier Qualifier() => new(new ContentLoader().Load("content"));

static EffectStackCommand Stack() => new(new EntityId(1), 1, new CombatEffect[]
{
    new DamageEffect(new EntityId(2), 3),
    new ApplyConditionEffect(new EntityId(2), ConditionKind.Marked, 2)
});

static CombatState State(BattleMap? map = null, params CombatEntity[] supplied)
{
    var source = new CombatEntity(new EntityId(1), new Cell(1, 1), new Footprint(1, 1), EntityFlags.Active, new IntegrityPool(10, 10));
    var targets = supplied.Length == 0 ? new[] { Target(2, 3, 1) } : supplied;
    return new CombatState(map ?? new BattleMap(8, 5), CombatRules.SpikeDefault, new[] { source }.Concat(targets), source.Id);
}

static CombatEntity Target(ulong id, int x, int y) =>
    new(new EntityId(id), new Cell(x, y), new Footprint(1, 1), EntityFlags.Active, new IntegrityPool(10, 10)) { Faction = Faction.Enemy };

static void Assert(bool condition, string message)
{
    if (!condition) throw new InvalidOperationException(message);
}

static void Throws<T>(Action action) where T : Exception
{
    try { action(); }
    catch (T) { return; }
    throw new InvalidOperationException($"Expected {typeof(T).Name}.");
}
