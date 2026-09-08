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
    ("forecast corpus stays within provisional time budget", ForecastBudget)
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
    var second = first.Reverse();
    Assert(new CombatState(first).DeterministicHash() == new CombatState(second).DeterministicHash(),
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
        var state = new CombatState(new[] { source, target });
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

    Assert(ReplayVerifier.FindFirstDivergence(expected.Events, actual.Events) == 1,
        "Verifier did not identify the second resolved event.");
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

static CombatResolver Resolver() => new();

static CombatState SampleState() => new(SampleEntities());

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
