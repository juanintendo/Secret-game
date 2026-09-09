namespace SecretGame.Simulation;

public enum CyborgSpecialtyPolicy
{
    Bulwark,
    RemoteArsenal,
    Redline
}

public static class SpecialtyPolicyExperimentScenario
{
    public static readonly EntityId Pilot = CyborgMechExperimentScenario.Pilot;
    public static readonly EntityId Mech = CyborgMechExperimentScenario.Mech;
    public static readonly EntityId Target = RemoteDirectiveExperimentScenario.Target;
    public static readonly ResourceId Charge = CyborgMechExperimentScenario.Charge;

    public static CombatState CreateBoarded()
    {
        var pilot = new CombatEntity(Pilot, new Cell(2, 3), new Footprint(1, 1), EntityFlags.Docked,
            new IntegrityPool(16, 16), 0, 11);
        var mech = new CombatEntity(Mech, new Cell(3, 3), new Footprint(2, 2), EntityFlags.Active,
            new IntegrityPool(30, 30), 0, 12) with { Mass = MassClass.Heavy };
        var target = new CombatEntity(Target, new Cell(7, 3), new Footprint(1, 1), EntityFlags.Active,
            new IntegrityPool(18, 18), 0, 10) with { Faction = Faction.Enemy };
        var resource = new SharedResourcePool(Charge, Pilot, Mech, "Charge", 4, 8);
        return new CombatState(new BattleMap(10, 7), CombatRules.SpikeDefault,
            new[] { pilot, mech, target }, resources: new[] { resource });
    }

    public static BoardedMechEffectStackCommand BulwarkAction() => new(
        Pilot,
        Mech,
        1,
        new CombatEffect[]
        {
            new SpendResourceEffect(Charge, 2),
            new GrantGuardEffect(Mech, 6)
        });

    public static BoardedMechEffectStackCommand RedlineAction() => new(
        Pilot,
        Mech,
        1,
        new CombatEffect[]
        {
            new DamageEffect(Target, 7)
        });

    public static IReadOnlyList<CombatCommand> BoardedScript(CyborgSpecialtyPolicy policy) => new CombatCommand[]
    {
        new BeginActivationCommand(Mech),
        policy switch
        {
            CyborgSpecialtyPolicy.Bulwark => BulwarkAction(),
            CyborgSpecialtyPolicy.Redline => RedlineAction(),
            _ => throw new ArgumentOutOfRangeException(nameof(policy), "Remote Arsenal uses the remote directive scenario.")
        },
        new EndActivationCommand(Mech)
    };
}
