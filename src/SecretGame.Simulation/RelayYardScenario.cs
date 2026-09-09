namespace SecretGame.Simulation;

public static class RelayYardScenario
{
    public static readonly EntityId Human = new(100);
    public static readonly EntityId Cyborg = new(200);
    public static readonly EntityId Mech = new(210);
    public static readonly EntityId Synthetic = new(300);
    public static readonly EntityId Warden = new(900);
    public static readonly EntityId Striker = new(901);
    public static readonly EntityId RelayTech = new(902);

    public static CombatState Create(Footprint? mechFootprint = null)
    {
        var terrain = Enumerable.Range(0, 12)
            .Where(y => y is not 5 and not 6)
            .Select(y => new TerrainTile(new Cell(6, y), 0, true))
            .Append(new TerrainTile(new Cell(9, 3), 1, false));
        var cover = new[]
        {
            new CoverEdge(new Cell(7, 5), new Cell(8, 5), CoverLevel.Full),
            new CoverEdge(new Cell(7, 7), new Cell(8, 7), CoverLevel.Half),
            new CoverEdge(new Cell(4, 5), new Cell(5, 5), CoverLevel.Half)
        };
        var map = new BattleMap(12, 12, terrain, cover);
        var entities = new[]
        {
            Unit(Human, new Cell(1, 5), 12, 9),
            Unit(Cyborg, new Cell(1, 7), 16, 11),
            Unit(Mech, new Cell(3, 8), 30, 12, mechFootprint ?? new Footprint(2, 2)) with { Mass = MassClass.Heavy },
            Unit(Synthetic, new Cell(2, 6), 10, 8),
            Unit(Warden, new Cell(8, 5), 18, 10) with { Faction = Faction.Enemy },
            Unit(Striker, new Cell(8, 7), 12, 7) with { Faction = Faction.Enemy },
            Unit(RelayTech, new Cell(9, 3), 9, 9) with { Faction = Faction.Enemy }
        };
        return new CombatState(map, CombatRules.SpikeDefault, entities);
    }

    public static IReadOnlyList<CombatCommand> ScriptedCommands() => new CombatCommand[]
    {
        new BeginActivationCommand(Human),
        new MoveCommand(Human, new Cell(4, 4), 3),
        new ApplyConditionCommand(Human, Warden, ConditionKind.Marked, 2),
        new EndActivationCommand(Human),
        new BeginActivationCommand(Cyborg),
        new SetDeploymentModeCommand(Cyborg, Mech, DeploymentMode.Docked),
        new EndActivationCommand(Cyborg),
        new BeginActivationCommand(Mech),
        new MoveCommand(Mech, new Cell(5, 5), 4),
        new DamageCommand(Mech, Striker, 7),
        new EndActivationCommand(Mech),
        new BeginActivationCommand(Synthetic),
        new MoveCommand(Synthetic, new Cell(4, 5), 2),
        new DamageCommand(Synthetic, Warden, 5),
        new EndActivationCommand(Synthetic),
        new BeginActivationCommand(Warden),
        new EndActivationCommand(Warden),
        new BeginActivationCommand(Striker),
        new EndActivationCommand(Striker),
        new BeginActivationCommand(RelayTech),
        new EndActivationCommand(RelayTech)
    };

    public static string NameOf(EntityId id)
    {
        if (id == Human) return "Human";
        if (id == Cyborg) return "Cyborg";
        if (id == Mech) return "Mech";
        if (id == Synthetic) return "Synthetic";
        if (id == Warden) return "Warden";
        if (id == Striker) return "Striker";
        if (id == RelayTech) return "RelayTech";
        return $"Entity-{id}";
    }

    private static CombatEntity Unit(
        EntityId id,
        Cell anchor,
        int integrity,
        int interval,
        Footprint? footprint = null) =>
        new(id, anchor, footprint ?? new Footprint(1, 1), EntityFlags.Active,
            new IntegrityPool(integrity, integrity), 0, interval);
}
