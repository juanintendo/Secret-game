namespace SecretGame.Simulation;

public static class CyborgMechExperimentScenario
{
    public static readonly EntityId Pilot = new(200);
    public static readonly EntityId Mech = new(210);
    public static readonly ResourceId Charge = new(1);

    public static CombatState Create()
    {
        var pilot = new CombatEntity(Pilot, new Cell(2, 2), new Footprint(1, 1), EntityFlags.Active,
            new IntegrityPool(16, 16), 0, 11);
        var mech = new CombatEntity(Mech, new Cell(3, 2), new Footprint(2, 2), EntityFlags.RemoteControlled,
            new IntegrityPool(30, 30), 0, 12) with { Mass = MassClass.Heavy };
        var charge = new SharedResourcePool(Charge, Pilot, Mech, "Charge", 3, 8);
        return new CombatState(new BattleMap(8, 6), CombatRules.SpikeDefault,
            new[] { pilot, mech }, resources: new[] { charge });
    }

    public static IReadOnlyList<CombatCommand> ScriptedCommands() => new CombatCommand[]
    {
        new BeginActivationCommand(Pilot),
        new BoardMechCommand(Pilot, Mech),
        new EndActivationCommand(Pilot),
        new BeginActivationCommand(Mech),
        new RechargeMechCommand(Pilot, Mech, Charge, 3),
        new DeployMechRemotelyCommand(Pilot, Mech, new Cell(2, 2), Charge, 2),
        new EndActivationCommand(Mech)
    };
}
