using System;
using System.Collections.Generic;
using System.Linq;
using SecretGame.Simulation;

namespace SecretGame.VisualSpike;

/// <summary>Fixture composition only. All rules are delegated to the frozen resolver.
/// No Unity types, visual time, quality settings or scale enter this replay.</summary>
public sealed class VisualReplay
{
    public static readonly EntityId Pilot = new(200), Mech = new(210), Enemy = new(300);
    public sealed class Step
    {
        public float Time { get; }
        public IReadOnlyList<ResolvedEvent> Events { get; }
        public IReadOnlyList<CombatEntity> Snapshot { get; }
        public string Hash { get; }
        public Step(float time, IReadOnlyList<ResolvedEvent> events, IReadOnlyList<CombatEntity> snapshot, string hash)
        { Time = time; Events = events; Snapshot = snapshot; Hash = hash; }
    }
    public IReadOnlyList<CombatEntity> Initial { get; }
    public IReadOnlyList<Step> Steps { get; }
    public ForecastResult AttackForecast { get; }
    public string FinalHash { get; }
    public const float MoveTime = 2, DockTime = 4, AttackTime = 8, Duration = 12;

    public VisualReplay()
    {
        var state = new CombatState(new BattleMap(8, 6), CombatRules.SpikeDefault,
            new[] {
                new CombatEntity(Pilot, new Cell(2, 1), new Footprint(1, 1), EntityFlags.Active, new IntegrityPool(16,16)),
                new CombatEntity(Mech, new Cell(3, 3), new Footprint(2, 2), EntityFlags.RemoteControlled, new IntegrityPool(30,30)) { Mass = MassClass.Heavy },
                new CombatEntity(Enemy, new Cell(5, 3), new Footprint(1, 1), EntityFlags.Active, new IntegrityPool(20,20)) { Faction = Faction.Enemy }
            }, resources: new[] { new SharedResourcePool(new ResourceId(1), Pilot, Mech, "Charge", 3, 8) });
        Initial = Array.AsReadOnly(state.Entities.Values.ToArray());
        var resolver = new CombatResolver();
        var steps = new List<Step>();
        void Resolve(float time, CombatCommand command)
        {
            var result = resolver.Resolve(state, command);
            steps.Add(new Step(time, Array.AsReadOnly(result.Events.ToArray()),
                Array.AsReadOnly(state.Entities.Values.ToArray()), result.ResultingStateHash));
        }
        Resolve(0, new BeginActivationCommand(Pilot));
        Resolve(MoveTime, new MoveCommand(Pilot, new Cell(2,3), 2));
        Resolve(DockTime, new BoardMechCommand(Pilot, Mech));
        Resolve(6, new EndActivationCommand(Pilot));
        Resolve(6, new BeginActivationCommand(Mech));
        var attack = new BoardedMechEffectStackCommand(Pilot, Mech, 1,
            new CombatEffect[] { new DamageEffect(Enemy, 7) });
        AttackForecast = new CombatForecast(resolver).Evaluate(state, attack);
        if (!AttackForecast.IsLegal) throw new InvalidOperationException(AttackForecast.RejectionReason);
        Resolve(AttackTime, attack);
        if (AttackForecast.ResultingStateHash != state.DeterministicHash())
            throw new InvalidOperationException("Forecast / execution divergence.");
        Resolve(10, new EndActivationCommand(Mech));
        Steps = steps.AsReadOnly();
        FinalHash = state.DeterministicHash();
    }

    public IReadOnlyList<CombatEntity> SnapshotAt(float time)
    {
        var snapshot = Initial;
        foreach (var step in Steps) { if (step.Time > time) break; snapshot = step.Snapshot; }
        return snapshot;
    }
}
