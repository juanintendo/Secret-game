namespace SecretGame.Simulation;

public enum DeploymentChoice
{
    Stay,
    Transition
}

public static class SpecialtyPressureExperimentScenario
{
    public static readonly EntityId Pilot = SpecialtyPolicyExperimentScenario.Pilot;
    public static readonly EntityId Mech = SpecialtyPolicyExperimentScenario.Mech;
    public static readonly EntityId Warden = SpecialtyPolicyExperimentScenario.Target;
    public static readonly ResourceId Charge = SpecialtyPolicyExperimentScenario.Charge;

    public static CombatState Create(CyborgSpecialtyPolicy policy)
    {
        var remote = policy == CyborgSpecialtyPolicy.Redline;
        var pilot = new CombatEntity(Pilot, new Cell(2, 3), new Footprint(1, 1),
            remote ? EntityFlags.Active : EntityFlags.Docked,
            new IntegrityPool(16, 16), remote ? 0 : 2, 10);
        var mech = new CombatEntity(Mech, new Cell(3, 3), new Footprint(2, 2),
            remote ? EntityFlags.RemoteControlled : EntityFlags.Active,
            new IntegrityPool(30, 30), remote ? 2 : 0, 10) with { Mass = MassClass.Heavy };
        var warden = new CombatEntity(Warden, new Cell(7, 3), new Footprint(1, 1), EntityFlags.Active,
            new IntegrityPool(24, 24), 1, 20) with { Faction = Faction.Enemy };
        return new CombatState(new BattleMap(10, 7), CombatRules.SpikeDefault,
            new[] { pilot, mech, warden }, resources: new[]
            {
                new SharedResourcePool(Charge, Pilot, Mech, "Charge", 4, 8)
            });
    }

    public static IReadOnlyList<CombatCommand> Script(
        CyborgSpecialtyPolicy policy,
        DeploymentChoice choice) => (policy, choice) switch
    {
        (CyborgSpecialtyPolicy.Bulwark, DeploymentChoice.Stay) => BoardedStay(BulwarkTwice()),
        (CyborgSpecialtyPolicy.Bulwark, DeploymentChoice.Transition) => BoardedToRemote(),
        (CyborgSpecialtyPolicy.RemoteArsenal, DeploymentChoice.Stay) => BoardedStay(RechargeTwice()),
        (CyborgSpecialtyPolicy.RemoteArsenal, DeploymentChoice.Transition) => BoardedToRemote(),
        (CyborgSpecialtyPolicy.Redline, DeploymentChoice.Stay) => RemoteStay(),
        (CyborgSpecialtyPolicy.Redline, DeploymentChoice.Transition) => RemoteToBoarded(),
        _ => throw new ArgumentOutOfRangeException(nameof(policy))
    };

    private static IReadOnlyList<CombatCommand> BoardedStay((CombatCommand First, CombatCommand Second) actions) =>
        new CombatCommand[]
        {
            new BeginActivationCommand(Mech), actions.First, new EndActivationCommand(Mech),
            new BeginActivationCommand(Warden), new DamageCommand(Warden, Mech, 8), new EndActivationCommand(Warden),
            new BeginActivationCommand(Mech), actions.Second, new EndActivationCommand(Mech)
        };

    private static IReadOnlyList<CombatCommand> BoardedToRemote() => new CombatCommand[]
    {
        new BeginActivationCommand(Mech),
        new DeployMechRemotelyCommand(Pilot, Mech, new Cell(2, 3), Charge, 2),
        new EndActivationCommand(Mech),
        new BeginActivationCommand(Warden), new DamageCommand(Warden, Mech, 8), new EndActivationCommand(Warden),
        new BeginActivationCommand(Pilot),
        new RemoteAttackDirectiveCommand(Pilot, Mech, Charge, Warden, 6, 1),
        new RemoteAttackDirectiveCommand(Pilot, Mech, Charge, Warden, 6, 1),
        new EndActivationCommand(Pilot)
    };

    private static IReadOnlyList<CombatCommand> RemoteStay() => new CombatCommand[]
    {
        new BeginActivationCommand(Pilot),
        new RemoteAttackDirectiveCommand(Pilot, Mech, Charge, Warden, 6, 1),
        new RemoteAttackDirectiveCommand(Pilot, Mech, Charge, Warden, 6, 1),
        new EndActivationCommand(Pilot),
        new BeginActivationCommand(Warden), new DamageCommand(Warden, Pilot, 8), new EndActivationCommand(Warden),
        new BeginActivationCommand(Pilot),
        new RemoteAttackDirectiveCommand(Pilot, Mech, Charge, Warden, 6, 1),
        new RemoteAttackDirectiveCommand(Pilot, Mech, Charge, Warden, 6, 1),
        new EndActivationCommand(Pilot)
    };

    private static IReadOnlyList<CombatCommand> RemoteToBoarded() => new CombatCommand[]
    {
        new BeginActivationCommand(Pilot), new BoardMechCommand(Pilot, Mech), new EndActivationCommand(Pilot),
        new BeginActivationCommand(Warden), new DamageCommand(Warden, Mech, 8), new EndActivationCommand(Warden),
        new BeginActivationCommand(Mech), SpecialtyPolicyExperimentScenario.RedlineAction(), new EndActivationCommand(Mech)
    };

    private static (CombatCommand First, CombatCommand Second) BulwarkTwice() =>
        (SpecialtyPolicyExperimentScenario.BulwarkAction(), SpecialtyPolicyExperimentScenario.BulwarkAction());

    private static (CombatCommand First, CombatCommand Second) RechargeTwice() =>
        (new RechargeMechCommand(Pilot, Mech, Charge, 3), new RechargeMechCommand(Pilot, Mech, Charge, 3));
}
