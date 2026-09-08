using SecretGame.Simulation;

var tests = new (string Name, Action Run)[]
{
    ("rectangular footprint enumerates stable cells", RectangularFootprint),
    ("docked pilot keeps ID but loses battlefield flags", DockedPilot),
    ("remote mode restores pilot battlefield flags", RemotePilot),
    ("state hash ignores insertion order", StableHash),
    ("Integrity is a bounded damage pool", IntegrityIsPool)
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
    state.SetPilotMechMode(new EntityId(10), new EntityId(20), DeploymentMode.Docked);
    var pilot = state.Entities[new EntityId(10)];
    Assert(!pilot.Flags.Spatial && !pilot.Flags.Selectable && !pilot.Flags.Targetable && !pilot.Flags.HasInitiativeSlot,
        "Docked pilot retained a battlefield capability.");
    Assert(state.Entities.ContainsKey(new EntityId(10)), "Pilot ID disappeared.");
}

static void RemotePilot()
{
    var state = SampleState();
    state.SetPilotMechMode(new EntityId(10), new EntityId(20), DeploymentMode.Docked);
    state.SetPilotMechMode(new EntityId(10), new EntityId(20), DeploymentMode.Remote);
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
