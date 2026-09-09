using SecretGame.Simulation;

var tests = new (string Name, Action Run)[]
{
    ("rectangular footprint enumerates stable cells", RectangularFootprint),
    ("docked pilot keeps ID but loses battlefield flags", DockedPilot),
    ("remote mode restores pilot battlefield flags", RemotePilot),
    ("state hash ignores insertion order", StableHash),
    ("Integrity is a bounded damage pool", IntegrityIsPool),
    ("forecast leaves authoritative state untouched", ForecastIsSpeculative),
    ("forecast and execution produce identical events", ForecastMatchesExecution),
    ("illegal forecast reports rejection without mutation", IllegalForecastIsSafe),
    ("replay reproduces deployment transitions and final hash", ReplayIsDeterministic),
    ("multi-cell movement rejects occupied destinations", OccupancyIsValidated),
    ("forecast parity holds across 500 state-command pairs", ForecastCorpusParity),
    ("replay verifier locates first divergent event", ReplayDivergenceIsLocated),
    ("forecast corpus stays within provisional time budget", ForecastBudget),
    ("movement topology remains configurable", MovementTopologyIsConfigurable),
    ("narrow door rejects 2x2 while allowing 1x1", NarrowDoorTestsFootprint),
    ("terrain blocks line of sight", TerrainBlocksSight),
    ("cover is read from the attacked tile edge", CoverComesFromEdge),
    ("initiative rail excludes docked pilot", InitiativeFiltersFlags),
    ("diagonal movement cannot cut blocked corners", DiagonalCannotCutCorner),
    ("initial state rejects overlapping entities", InitialOverlapIsRejected),
    ("movement forecast includes the exact executed path", MovementForecastMatchesExecution),
    ("map and movement rules participate in state hash", SpatialRulesAffectHash),
    ("two actions exhaust AP until activation refresh", ActionEconomyIsEnforced),
    ("movement forecast reports closed isolation windows", ForecastReportsClosedConditions),
    ("elevation forecast reports opened condition", ForecastReportsOpenedElevation),
    ("applied and derived conditions remain separate", AppliedAndDerivedStaySeparate),
    ("hostile adjacency derives Surrounded", SurroundedIsDerived),
    ("initiative scheduler rejects out-of-order activation", ActivationOrderIsEnforced),
    ("applied condition expires on owner activations", ConditionDurationAdvances),
    ("Relay Yard script preserves forecast parity", RelayYardForecastParity),
    ("Relay Yard replay is deterministic", RelayYardReplayIsDeterministic),
    ("text renderer exposes tactical windows", TextRendererShowsWindows),
    ("resolver rejects attacks through blocked LOS", ResolverEnforcesLineOfSight),
    ("shared resource participates in clone and deterministic hash", SharedResourceIsAuthoritative),
    ("Board Mech spends AP without refilling Charge", BoardMechIsForecastable),
    ("Board Mech requires physical adjacency", BoardMechRequiresAdjacency),
    ("docked recharge is bounded and forecastable", DockedRechargeIsBounded),
    ("Deploy Remotely spends Charge and removes mech initiative", RemoteDeploymentUsesSharedBudget),
    ("insufficient Charge rejects remote deployment atomically", InsufficientChargeIsAtomic),
    ("legacy Gate A replay hashes remain unchanged", GateAHashesRemainStable)
};

var failures = 0;
foreach (var test in tests)
{
    try { test.Run(); Console.WriteLine($"PASS {test.Name}"); }
    catch (Exception error) { failures++; Console.Error.WriteLine($"FAIL {test.Name}: {error.Message}"); }
}

Console.WriteLine($"{tests.Length - failures}/{tests.Length} tests passed");
return failures == 0 ? 0 : 1;

static void RectangularFootprint()
{
    var cells = new Footprint(2, 2).OccupiedCells(new Cell(4, 7)).ToArray();
    Equal(new[] { new Cell(4, 7), new Cell(5, 7), new Cell(4, 8), new Cell(5, 8) }, cells);
}

static void DockedPilot()
{
    var state = SampleState();
    Resolver().Resolve(state, new SetDeploymentModeCommand(new EntityId(10), new EntityId(20), DeploymentMode.Docked));
    var pilot = state.Entities[new EntityId(10)];
    Assert(!pilot.Flags.Spatial && !pilot.Flags.Selectable && !pilot.Flags.Targetable && !pilot.Flags.HasInitiativeSlot,
        "Docked pilot retained a battlefield capability.");
    Assert(state.Entities.ContainsKey(new EntityId(10)), "Pilot ID disappeared.");
}

static void RemotePilot()
{
    var state = SampleState();
    var resolver = Resolver();
    resolver.Resolve(state, new SetDeploymentModeCommand(new EntityId(10), new EntityId(20), DeploymentMode.Docked));
    resolver.Resolve(state, new SetDeploymentModeCommand(new EntityId(10), new EntityId(20), DeploymentMode.Remote));
    Assert(state.Entities[new EntityId(10)].Flags == EntityFlags.Active, "Pilot flags were not restored.");
}

static void StableHash()
{
    var first = SampleEntities().ToArray();
    var second = first.AsEnumerable().Reverse();
    Assert(
        new CombatState(OpenMap(), CombatRules.SpikeDefault, first).DeterministicHash()
        == new CombatState(OpenMap(), CombatRules.SpikeDefault, second).DeterministicHash(),
        "Hash changed with insertion order.");
}

static void IntegrityIsPool()
{
    var pool = new IntegrityPool(9, 12).ApplyDamage(20);
    Assert(pool.Current == 0 && pool.Maximum == 12, "Damage pool did not clamp at zero.");
}

static void ForecastIsSpeculative()
{
    var state = SampleState();
    var before = state.DeterministicHash();
    var forecast = new CombatForecast(Resolver()).Evaluate(
        state,
        new DamageCommand(new EntityId(10), new EntityId(20), 7));

    Assert(forecast.IsLegal, "Expected legal forecast.");
    Assert(state.DeterministicHash() == before, "Forecast mutated authoritative state.");
    Assert(state.Events.Count == 0, "Forecast appended an authoritative event.");
}

static void ForecastMatchesExecution()
{
    var state = SampleState();
    var command = new DamageCommand(new EntityId(10), new EntityId(20), 7);
    var resolver = Resolver();
    var forecast = new CombatForecast(resolver).Evaluate(state, command);
    var actual = resolver.Resolve(state, command);

    Assert(forecast.IsLegal, "Expected legal forecast.");
    Equal(forecast.Events, actual.Events);
    Assert(forecast.ResultingStateHash == actual.ResultingStateHash, "Forecast hash differs from execution.");
}

static void IllegalForecastIsSafe()
{
    var state = SampleState();
    var before = state.DeterministicHash();
    var forecast = new CombatForecast(Resolver()).Evaluate(
        state,
        new MoveCommand(new EntityId(10), new Cell(3, 3)));

    Assert(!forecast.IsLegal, "Overlapping move was forecast as legal.");
    Assert(forecast.RejectionReason is not null, "Rejected forecast has no reason.");
    Assert(state.DeterministicHash() == before, "Rejected forecast mutated state.");
}

static void ReplayIsDeterministic()
{
    var commands = new CombatCommand[]
    {
        new SetDeploymentModeCommand(new EntityId(10), new EntityId(20), DeploymentMode.Docked),
        new SetDeploymentModeCommand(new EntityId(10), new EntityId(20), DeploymentMode.Remote),
        new MoveCommand(new EntityId(10), new Cell(0, 4)),
        new DamageCommand(new EntityId(10), new EntityId(20), 6)
    };
    var replay = new CombatReplay(Resolver());
    var first = replay.Run(SampleState(), commands);
    var second = replay.Run(SampleState(), commands);

    Equal(first.Events, second.Events);
    Assert(first.FinalStateHash == second.FinalStateHash, "Replay final hashes differ.");
    Assert(first.FinalState.Entities.ContainsKey(new EntityId(10)), "Replay lost pilot ID.");
    Assert(first.FinalState.Entities.ContainsKey(new EntityId(20)), "Replay lost mech ID.");
}

static void OccupancyIsValidated()
{
    var state = SampleState();
    Throws<CommandRejectedException>(() =>
        Resolver().Resolve(state, new MoveCommand(new EntityId(20), new Cell(0, 0))));
}

static void ForecastCorpusParity()
{
    var resolver = Resolver();
    var forecast = new CombatForecast(resolver);

    for (var index = 1; index <= 500; index++)
    {
        var source = new CombatEntity(
            new EntityId(1),
            new Cell(index % 7, index % 11),
            new Footprint(1, 1),
            EntityFlags.Active,
            new IntegrityPool(1000, 1000));
        var target = new CombatEntity(
            new EntityId(2),
            new Cell(20 + index % 5, 20 + index % 3),
            new Footprint(index % 2 + 1, index % 3 + 1),
            EntityFlags.Active,
            new IntegrityPool(1000, 1000));
        var state = new CombatState(OpenMap(), CombatRules.SpikeDefault, new[] { source, target }, source.Id);
        var command = new DamageCommand(source.Id, target.Id, index);
        var predicted = forecast.Evaluate(state, command);
        var actual = resolver.Resolve(state, command);

        Assert(predicted.IsLegal, $"Pair {index} was unexpectedly illegal.");
        Equal(predicted.Events, actual.Events);
        Assert(predicted.ResultingStateHash == actual.ResultingStateHash, $"Pair {index} hash differs.");
    }
}

static void ReplayDivergenceIsLocated()
{
    var replay = new CombatReplay(Resolver());
    var sharedOpening = new CombatCommand[]
    {
        new MoveCommand(new EntityId(10), new Cell(0, 4)),
        new DamageCommand(new EntityId(10), new EntityId(20), 4)
    };
    var changedSecondAction = new CombatCommand[]
    {
        new MoveCommand(new EntityId(10), new Cell(0, 4)),
        new DamageCommand(new EntityId(10), new EntityId(20), 5)
    };
    var expected = replay.Run(SampleState(), sharedOpening);
    var actual = replay.Run(SampleState(), changedSecondAction);

    Assert(ReplayVerifier.FindFirstDivergence(expected.Events, actual.Events) == 3,
        "Verifier did not identify the fourth resolved event.");
    Assert(ReplayVerifier.FindFirstDivergence(expected.Events, expected.Events) is null,
        "Verifier reported divergence for identical traces.");
}

static void ForecastBudget()
{
    var resolver = Resolver();
    var forecast = new CombatForecast(resolver);
    var state = SampleState();
    var command = new DamageCommand(new EntityId(10), new EntityId(20), 1);
    var timer = System.Diagnostics.Stopwatch.StartNew();

    for (var index = 0; index < 500; index++)
        Assert(forecast.Evaluate(state, command).IsLegal, "Benchmark command became illegal.");

    timer.Stop();
    Assert(timer.ElapsedMilliseconds <= 2000,
        $"500 forecasts exceeded the provisional aggregate budget: {timer.ElapsedMilliseconds} ms.");
}

static void MovementTopologyIsConfigurable()
{
    var entity = new CombatEntity(new EntityId(1), new Cell(0, 0), new Footprint(1, 1), EntityFlags.Active, new IntegrityPool(5, 5));
    var cardinalState = new CombatState(OpenMap(), new CombatRules(MovementTopology.CardinalFour, 1), new[] { entity });
    var diagonalState = new CombatState(OpenMap(), new CombatRules(MovementTopology.EightConnected, 1), new[] { entity });
    var pathfinder = new GridPathfinder();

    Assert(pathfinder.FindPath(cardinalState, entity, new Cell(1, 1))?.StepCount == 2,
        "Cardinal topology did not require two steps.");
    Assert(pathfinder.FindPath(diagonalState, entity, new Cell(1, 1))?.StepCount == 1,
        "Eight-connected topology did not permit one diagonal step.");
}

static void NarrowDoorTestsFootprint()
{
    var wall = Enumerable.Range(0, 5)
        .Where(y => y != 2)
        .Select(y => new TerrainTile(new Cell(3, y), 0, true));
    var map = new BattleMap(7, 5, wall);
    var human = new CombatEntity(new EntityId(1), new Cell(1, 2), new Footprint(1, 1), EntityFlags.Active, new IntegrityPool(5, 5));
    var mech = new CombatEntity(new EntityId(2), new Cell(0, 0), new Footprint(2, 2), EntityFlags.Active, new IntegrityPool(20, 20));
    var pathfinder = new GridPathfinder();

    var humanState = new CombatState(map, CombatRules.SpikeDefault, new[] { human });
    var mechState = new CombatState(map, CombatRules.SpikeDefault, new[] { mech });
    Assert(pathfinder.FindPath(humanState, human, new Cell(5, 2)) is not null,
        "Human could not use one-cell door.");
    Assert(pathfinder.FindPath(mechState, mech, new Cell(4, 0)) is null,
        "Mech incorrectly crossed one-cell door.");
}

static void TerrainBlocksSight()
{
    var blockedMap = new BattleMap(8, 8, new[] { new TerrainTile(new Cell(2, 1), 0, true) });
    var sight = new LineOfSight();
    Assert(!sight.HasClearLine(blockedMap, new Cell(1, 1), new Cell(3, 1)),
        "Blocked terrain did not stop sight.");
    Assert(sight.HasClearLine(blockedMap, new Cell(1, 2), new Cell(3, 2)),
        "Clear row incorrectly stopped sight.");
}

static void CoverComesFromEdge()
{
    var target = new Cell(4, 4);
    var map = new BattleMap(8, 8, cover: new[]
    {
        new CoverEdge(target, new Cell(3, 4), CoverLevel.Full),
        new CoverEdge(target, new Cell(4, 3), CoverLevel.Half)
    });
    var resolver = new CoverResolver();

    Assert(resolver.AgainstAttack(map, target, new Cell(0, 4)) == CoverLevel.Full,
        "West attack did not read west edge.");
    Assert(resolver.AgainstAttack(map, target, new Cell(4, 0)) == CoverLevel.Half,
        "North attack did not read north edge.");
}

static void InitiativeFiltersFlags()
{
    var state = SampleState();
    var resolver = Resolver();
    resolver.Resolve(state, new SetDeploymentModeCommand(new EntityId(10), new EntityId(20), DeploymentMode.Docked));
    var rail = new InitiativeTimeline().Query(state, 4);

    Assert(rail.All(slot => slot.EntityId == new EntityId(20)), "Docked pilot appeared on initiative rail.");
    Assert(rail.Select(slot => slot.ActsAt).SequenceEqual(new[] { 0, 10, 20, 30 }),
        "Initiative rail did not advance by the mech interval.");
}

static void DiagonalCannotCutCorner()
{
    var map = new BattleMap(4, 4, new[]
    {
        new TerrainTile(new Cell(1, 0), 0, true),
        new TerrainTile(new Cell(0, 1), 0, true)
    });
    var entity = new CombatEntity(new EntityId(1), new Cell(0, 0), new Footprint(1, 1), EntityFlags.Active, new IntegrityPool(5, 5));
    var state = new CombatState(map, CombatRules.SpikeDefault, new[] { entity }, entity.Id);

    Assert(new GridPathfinder().FindPath(state, entity, new Cell(1, 1)) is null,
        "Diagonal path cut through two blocked corners.");
}

static void InitialOverlapIsRejected()
{
    var first = new CombatEntity(new EntityId(1), new Cell(1, 1), new Footprint(2, 2), EntityFlags.Active, new IntegrityPool(5, 5));
    var second = new CombatEntity(new EntityId(2), new Cell(2, 2), new Footprint(1, 1), EntityFlags.Active, new IntegrityPool(5, 5));
    Throws<ArgumentException>(() =>
        new CombatState(OpenMap(), CombatRules.SpikeDefault, new[] { first, second }));
}

static void MovementForecastMatchesExecution()
{
    var state = SampleState();
    var command = new MoveCommand(new EntityId(10), new Cell(0, 4), 4);
    var resolver = Resolver();
    var forecast = new CombatForecast(resolver).Evaluate(state, command);
    var actual = resolver.Resolve(state, command);

    Assert(forecast.IsLegal, "Expected movement forecast to be legal.");
    Equal(forecast.Events, actual.Events);
    var moved = (EntityMovedEvent)actual.Events[1].Payload;
    Assert(moved.Path.StepCount == 3, "Unexpected movement path length.");
}

static void SpatialRulesAffectHash()
{
    var entities = SampleEntities().ToArray();
    var cardinal = new CombatState(OpenMap(), new CombatRules(MovementTopology.CardinalFour, 1), entities);
    var diagonal = new CombatState(OpenMap(), new CombatRules(MovementTopology.EightConnected, 1), entities);
    var blockedMap = new BattleMap(32, 32, new[] { new TerrainTile(new Cell(20, 20), 0, true) });
    var terrainChanged = new CombatState(blockedMap, new CombatRules(MovementTopology.CardinalFour, 1), entities);

    Assert(cardinal.DeterministicHash() != diagonal.DeterministicHash(), "Movement topology was absent from hash.");
    Assert(cardinal.DeterministicHash() != terrainChanged.DeterministicHash(), "Map definition was absent from hash.");
}

static void ActionEconomyIsEnforced()
{
    var state = SampleState();
    var resolver = Resolver();
    resolver.Resolve(state, new DamageCommand(new EntityId(10), new EntityId(20), 1));
    resolver.Resolve(state, new DamageCommand(new EntityId(10), new EntityId(20), 1));
    Throws<CommandRejectedException>(() =>
        resolver.Resolve(state, new DamageCommand(new EntityId(10), new EntityId(20), 1)));
    resolver.Resolve(state, new EndActivationCommand(new EntityId(10)));
    resolver.Resolve(state, new BeginActivationCommand(new EntityId(10)));
    Assert(state.Entities[new EntityId(10)].ActionPoints == 2, "Activation did not refresh two AP.");
}

static void ForecastReportsClosedConditions()
{
    var state = SampleState();
    var forecast = new CombatForecast(Resolver()).Evaluate(
        state,
        new MoveCommand(new EntityId(10), new Cell(2, 2), 1));

    Assert(forecast.IsLegal, "Expected joining move to be legal.");
    Assert(forecast.ConditionsClosed.Contains(new ConditionSignal(new EntityId(10), ConditionKind.Isolated)),
        "Forecast did not close pilot isolation.");
    Assert(forecast.ConditionsClosed.Contains(new ConditionSignal(new EntityId(20), ConditionKind.Isolated)),
        "Forecast did not close mech isolation.");
}

static void ForecastReportsOpenedElevation()
{
    var map = new BattleMap(5, 5, new[] { new TerrainTile(new Cell(1, 0), 1, false) });
    var entity = new CombatEntity(new EntityId(1), new Cell(0, 0), new Footprint(1, 1), EntityFlags.Active, new IntegrityPool(5, 5));
    var state = new CombatState(map, CombatRules.SpikeDefault, new[] { entity }, entity.Id);
    var forecast = new CombatForecast(Resolver()).Evaluate(state, new MoveCommand(entity.Id, new Cell(1, 0), 1));

    Assert(forecast.IsLegal, "Expected elevation move to be legal.");
    Assert(forecast.ConditionsOpened.Contains(new ConditionSignal(entity.Id, ConditionKind.Elevated)),
        "Forecast did not open Elevated.");
}

static void AppliedAndDerivedStaySeparate()
{
    var state = SampleState();
    var resolver = Resolver();
    resolver.Resolve(state, new ApplyConditionCommand(new EntityId(10), new EntityId(20), ConditionKind.Marked, 2));
    Assert(state.Entities[new EntityId(20)].Conditions.Items.Any(item => item.Kind == ConditionKind.Marked),
        "Applied condition was not stored.");
    Throws<CommandRejectedException>(() =>
        resolver.Resolve(state, new ApplyConditionCommand(new EntityId(10), new EntityId(20), ConditionKind.Isolated, 2)));
}

static void SurroundedIsDerived()
{
    var player = new CombatEntity(new EntityId(1), new Cell(2, 2), new Footprint(1, 1), EntityFlags.Active, new IntegrityPool(5, 5));
    var enemyA = new CombatEntity(new EntityId(2), new Cell(1, 2), new Footprint(1, 1), EntityFlags.Active, new IntegrityPool(5, 5))
        with { Faction = Faction.Enemy };
    var enemyB = new CombatEntity(new EntityId(3), new Cell(3, 2), new Footprint(1, 1), EntityFlags.Active, new IntegrityPool(5, 5))
        with { Faction = Faction.Enemy };
    var state = new CombatState(OpenMap(), CombatRules.SpikeDefault, new[] { player, enemyA, enemyB });

    Assert(new ConditionEvaluator().Evaluate(state)
        .Contains(new ConditionSignal(player.Id, ConditionKind.Surrounded)),
        "Two adjacent hostiles did not derive Surrounded.");
}

static void ActivationOrderIsEnforced()
{
    var state = new CombatState(OpenMap(), CombatRules.SpikeDefault, SampleEntities());
    Throws<CommandRejectedException>(() =>
        Resolver().Resolve(state, new BeginActivationCommand(new EntityId(20))));
    Resolver().Resolve(state, new BeginActivationCommand(new EntityId(10)));
    Assert(new InitiativeTimeline().Query(state, 1)[0].EntityId == new EntityId(20),
        "Scheduler did not advance to the next entity.");
}

static void ConditionDurationAdvances()
{
    var state = SampleState();
    var resolver = Resolver();
    resolver.Resolve(state, new ApplyConditionCommand(new EntityId(10), new EntityId(20), ConditionKind.Marked, 2));
    resolver.Resolve(state, new EndActivationCommand(new EntityId(10)));
    resolver.Resolve(state, new BeginActivationCommand(new EntityId(10)));
    resolver.Resolve(state, new EndActivationCommand(new EntityId(10)));
    resolver.Resolve(state, new BeginActivationCommand(new EntityId(20)));
    Assert(state.Entities[new EntityId(20)].Conditions.Items.Single().RemainingActivations == 1,
        "Condition did not advance to one remaining activation.");
    resolver.Resolve(state, new EndActivationCommand(new EntityId(20)));
    resolver.Resolve(state, new BeginActivationCommand(new EntityId(10)));
    resolver.Resolve(state, new EndActivationCommand(new EntityId(10)));
    var expiration = new CombatForecast(resolver).Evaluate(state, new BeginActivationCommand(new EntityId(20)));
    Assert(expiration.ConditionsClosed.Contains(new ConditionSignal(new EntityId(20), ConditionKind.Marked)),
        "Forecast did not report condition expiration.");
    resolver.Resolve(state, new BeginActivationCommand(new EntityId(20)));
    Assert(state.Entities[new EntityId(20)].Conditions.Items.Count == 0, "Expired condition remained stored.");
}

static void RelayYardForecastParity()
{
    var state = RelayYardScenario.Create();
    var resolver = Resolver();
    var forecast = new CombatForecast(resolver);
    foreach (var command in RelayYardScenario.ScriptedCommands())
    {
        var predicted = forecast.Evaluate(state, command);
        Assert(predicted.IsLegal, $"Relay Yard command rejected: {predicted.RejectionReason}");
        var actual = resolver.Resolve(state, command);
        Equal(predicted.Events, actual.Events);
        Assert(predicted.ResultingStateHash == actual.ResultingStateHash,
            "Relay Yard forecast hash differs from execution.");
    }
}

static void RelayYardReplayIsDeterministic()
{
    var replay = new CombatReplay(Resolver());
    var first = replay.Run(RelayYardScenario.Create(), RelayYardScenario.ScriptedCommands());
    var second = replay.Run(RelayYardScenario.Create(), RelayYardScenario.ScriptedCommands());
    Equal(first.Events, second.Events);
    Assert(first.FinalStateHash == second.FinalStateHash, "Relay Yard final hash changed between replays.");
}

static void TextRendererShowsWindows()
{
    var state = RelayYardScenario.Create();
    var resolver = Resolver();
    resolver.Resolve(state, RelayYardScenario.ScriptedCommands()[0]);
    var forecast = new CombatForecast(resolver).Evaluate(state, RelayYardScenario.ScriptedCommands()[1]);
    var text = new TextForecastRenderer().RenderForecast(forecast);
    Assert(text.Contains("OPENS Human:Isolated", StringComparison.Ordinal),
        "Text forecast omitted opened tactical window.");
    Assert(text.Contains("AP Human: 2 -> 1", StringComparison.Ordinal),
        "Text forecast omitted action-point cost.");

    var teaching = new TextForecastRenderer().RenderTeachingState(state);
    Assert(teaching.Contains("X  0  1  2  3", StringComparison.Ordinal),
        "Teaching map omitted zero-based X coordinates.");
    Assert(teaching.Contains("  0 |", StringComparison.Ordinal),
        "Teaching map omitted zero-based Y coordinates.");
    Assert(teaching.Contains("W Warden | K Striker | T RelayTech", StringComparison.Ordinal),
        "Teaching map did not distinguish enemy identities.");
    Assert(teaching.Contains("M Mech", StringComparison.Ordinal),
        "Teaching map omitted the mech legend.");
}

static void ResolverEnforcesLineOfSight()
{
    var map = new BattleMap(5, 3, new[] { new TerrainTile(new Cell(2, 1), 0, true) });
    var source = new CombatEntity(new EntityId(1), new Cell(1, 1), new Footprint(1, 1), EntityFlags.Active, new IntegrityPool(5, 5));
    var target = new CombatEntity(new EntityId(2), new Cell(3, 1), new Footprint(1, 1), EntityFlags.Active, new IntegrityPool(5, 5))
        with { Faction = Faction.Enemy };
    var state = new CombatState(map, CombatRules.SpikeDefault, new[] { source, target }, source.Id);

    Throws<CommandRejectedException>(() =>
        Resolver().Resolve(state, new DamageCommand(source.Id, target.Id, 1)));
}

static void SharedResourceIsAuthoritative()
{
    var state = CyborgMechExperimentScenario.Create();
    var clone = state.Clone();
    Assert(clone.Resources.Count == 1, "Clone lost the shared resource.");
    Assert(clone.Resources[CyborgMechExperimentScenario.Charge].OwnerId == CyborgMechExperimentScenario.Pilot,
        "Shared resource lost its pilot/mech identity owner.");
    Assert(clone.DeterministicHash() == state.DeterministicHash(), "Clone changed resource-bearing state hash.");

    var changed = new CombatState(state.Map, state.Rules, state.Entities.Values,
        resources: new[] { new SharedResourcePool(CyborgMechExperimentScenario.Charge,
            CyborgMechExperimentScenario.Pilot, CyborgMechExperimentScenario.Mech, "Charge", 4, 8) });
    Assert(changed.DeterministicHash() != state.DeterministicHash(), "Resource value was absent from state hash.");
}

static void BoardMechIsForecastable()
{
    var state = CyborgMechExperimentScenario.Create();
    var resolver = Resolver();
    resolver.Resolve(state, new BeginActivationCommand(CyborgMechExperimentScenario.Pilot));
    var command = new BoardMechCommand(CyborgMechExperimentScenario.Pilot, CyborgMechExperimentScenario.Mech);
    var predicted = new CombatForecast(resolver).Evaluate(state, command);
    var actual = resolver.Resolve(state, command);

    Equal(predicted.Events, actual.Events);
    Assert(predicted.ResultingStateHash == actual.ResultingStateHash, "Board forecast hash diverged.");
    Assert(state.Resources[CyborgMechExperimentScenario.Charge].Current == 3,
        "Boarding became a free battery refill.");
    Assert(state.Entities[CyborgMechExperimentScenario.Pilot].Flags == EntityFlags.Docked,
        "Board Mech did not remove the pilot from the map.");
}

static void DockedRechargeIsBounded()
{
    var state = DockedExperimentState(7);
    var command = new RechargeMechCommand(CyborgMechExperimentScenario.Pilot,
        CyborgMechExperimentScenario.Mech, CyborgMechExperimentScenario.Charge, 3);
    var resolver = Resolver();
    var predicted = new CombatForecast(resolver).Evaluate(state, command);
    var actual = resolver.Resolve(state, command);

    Equal(predicted.Events, actual.Events);
    Assert(state.Resources[CyborgMechExperimentScenario.Charge].Current == 8,
        "Recharge did not clamp at its maximum.");
    var delta = (ResourceChangedEvent)actual.Events[1].Payload;
    Assert(delta.Amount == 1 && delta.Before == 7 && delta.After == 8,
        "Recharge forecast did not expose the exact bounded delta.");
}

static void BoardMechRequiresAdjacency()
{
    var baseState = CyborgMechExperimentScenario.Create();
    var distantPilot = baseState.Entities[CyborgMechExperimentScenario.Pilot] with { Anchor = new Cell(0, 0) };
    var state = new CombatState(baseState.Map, baseState.Rules,
        new[] { distantPilot, baseState.Entities[CyborgMechExperimentScenario.Mech] }, distantPilot.Id,
        baseState.Resources.Values);
    var before = state.DeterministicHash();
    var forecast = new CombatForecast(Resolver()).Evaluate(state,
        new BoardMechCommand(CyborgMechExperimentScenario.Pilot, CyborgMechExperimentScenario.Mech));
    Assert(!forecast.IsLegal, "Distant boarding was forecast as legal.");
    Assert(state.DeterministicHash() == before, "Rejected distant boarding mutated state.");
}

static void RemoteDeploymentUsesSharedBudget()
{
    var state = DockedExperimentState(5);
    var command = new DeployMechRemotelyCommand(CyborgMechExperimentScenario.Pilot,
        CyborgMechExperimentScenario.Mech, new Cell(2, 2), CyborgMechExperimentScenario.Charge, 2);
    var resolver = Resolver();
    var predicted = new CombatForecast(resolver).Evaluate(state, command);
    var actual = resolver.Resolve(state, command);

    Equal(predicted.Events, actual.Events);
    Assert(state.Resources[CyborgMechExperimentScenario.Charge].Current == 3,
        "Remote deployment did not spend shared Charge.");
    Assert(state.Entities[CyborgMechExperimentScenario.Pilot].Flags == EntityFlags.Active,
        "Remote deployment did not restore the pilot to the map.");
    Assert(state.Entities[CyborgMechExperimentScenario.Pilot].Anchor == new Cell(2, 2),
        "Remote deployment did not place the pilot at the forecast destination.");
    Assert(state.Entities[CyborgMechExperimentScenario.Mech].Flags == EntityFlags.RemoteControlled,
        "Remote mech incorrectly retained an unrestricted initiative slot.");
    var text = new TextForecastRenderer().RenderForecast(predicted);
    Assert(text.Contains("RESOURCE Charge (Deployment): 5 -> 3 (-2)", StringComparison.Ordinal),
        "Text forecast omitted the exact Charge spend.");
    Assert(text.Contains("pilot -> (2,2)", StringComparison.Ordinal),
        "Text forecast omitted the pilot deployment destination.");
}

static void InsufficientChargeIsAtomic()
{
    var state = DockedExperimentState(1);
    var before = state.DeterministicHash();
    var forecast = new CombatForecast(Resolver()).Evaluate(state,
        new DeployMechRemotelyCommand(CyborgMechExperimentScenario.Pilot,
            CyborgMechExperimentScenario.Mech, new Cell(2, 2), CyborgMechExperimentScenario.Charge, 2));

    Assert(!forecast.IsLegal, "Insufficient Charge was forecast as legal.");
    Assert(state.DeterministicHash() == before && state.Events.Count == 0,
        "Rejected deployment partially mutated authoritative state.");
}

static void GateAHashesRemainStable()
{
    var replay = new CombatReplay(Resolver());
    var one = replay.Run(RelayYardScenario.Create(new Footprint(1, 1)), RelayYardScenario.ScriptedCommands());
    var two = replay.Run(RelayYardScenario.Create(new Footprint(2, 2)), RelayYardScenario.ScriptedCommands());
    Assert(one.FinalStateHash == "716BE2E159FBB184B422C33910A6A0513FEF8B034D53E531F76E8A2BAACAF0B4",
        "1x1 Gate A replay hash changed.");
    Assert(two.FinalStateHash == "533CEDD457740A8604C19265EFEF864E361325152A10777E32C1C72D62260689",
        "2x2 Gate A replay hash changed.");
}

static CombatState DockedExperimentState(int charge)
{
    var baseState = CyborgMechExperimentScenario.Create();
    var pilot = baseState.Entities[CyborgMechExperimentScenario.Pilot] with { Flags = EntityFlags.Docked };
    var mech = baseState.Entities[CyborgMechExperimentScenario.Mech] with { Flags = EntityFlags.Active };
    return new CombatState(baseState.Map, baseState.Rules, new[] { pilot, mech }, mech.Id,
        new[] { new SharedResourcePool(CyborgMechExperimentScenario.Charge,
            CyborgMechExperimentScenario.Pilot, CyborgMechExperimentScenario.Mech, "Charge", charge, 8) });
}

static CombatResolver Resolver() => new();

static CombatState SampleState() => new(
    OpenMap(),
    CombatRules.SpikeDefault,
    SampleEntities(),
    new EntityId(10));

static BattleMap OpenMap() => new(32, 32);

static IEnumerable<CombatEntity> SampleEntities()
{
    yield return new CombatEntity(new EntityId(20), new Cell(3, 3), new Footprint(2, 2), EntityFlags.Active, new IntegrityPool(30, 30));
    yield return new CombatEntity(new EntityId(10), new Cell(1, 1), new Footprint(1, 1), EntityFlags.Active, new IntegrityPool(12, 12));
}

static void Assert(bool condition, string message)
{
    if (!condition) throw new InvalidOperationException(message);
}

static void Equal<T>(IReadOnlyList<T> expected, IReadOnlyList<T> actual)
{
    Assert(expected.Count == actual.Count, "Sequence length differs.");
    for (var index = 0; index < expected.Count; index++)
        Assert(EqualityComparer<T>.Default.Equals(expected[index], actual[index]), $"Sequence differs at {index}.");
}

static void Throws<TException>(Action action) where TException : Exception
{
    try { action(); }
    catch (TException) { return; }
    throw new InvalidOperationException($"Expected {typeof(TException).Name}.");
}
