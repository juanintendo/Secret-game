namespace SecretGame.Simulation;

public static class RemoteDirectiveExperimentScenario
{
    public static readonly EntityId Pilot = CyborgMechExperimentScenario.Pilot;
    public static readonly EntityId Mech = CyborgMechExperimentScenario.Mech;
    public static readonly EntityId Target = new(900);
    public static readonly ResourceId Charge = CyborgMechExperimentScenario.Charge;

    public static CombatState Create(int charge = 4)
    {
        var pilot = new CombatEntity(Pilot, new Cell(1, 2), new Footprint(1, 1), EntityFlags.Active,
            new IntegrityPool(16, 16), 0, 11);
        var mech = new CombatEntity(Mech, new Cell(3, 3), new Footprint(2, 2), EntityFlags.RemoteControlled,
            new IntegrityPool(30, 30), 0, 12) with { Mass = MassClass.Heavy };
        var target = new CombatEntity(Target, new Cell(7, 3), new Footprint(1, 1), EntityFlags.Active,
            new IntegrityPool(18, 18), 0, 10) with { Faction = Faction.Enemy };
        var resource = new SharedResourcePool(Charge, Pilot, Mech, "Charge", charge, 8);
        return new CombatState(new BattleMap(10, 7), CombatRules.SpikeDefault,
            new[] { pilot, mech, target }, resources: new[] { resource });
    }

    public static IReadOnlyList<CombatCommand> ScriptedCommands() => new CombatCommand[]
    {
        new BeginActivationCommand(Pilot),
        new RemoteMoveDirectiveCommand(Pilot, Mech, Charge, new Cell(5, 3), 2, 1),
        new RemoteAttackDirectiveCommand(Pilot, Mech, Charge, Target, 6, 1),
        new EndActivationCommand(Pilot)
    };
}
