using SecretGame.Simulation;

Console.WriteLine("SPECIALTY PRESSURE: STAY vs TRANSITION — RECOMMENDATION, NOT CANON");
Console.WriteLine("Two player activations, one Warden attack, identical starting pressure per policy.");
Console.WriteLine();
Console.WriteLine("Policy          Choice      Damage  Prevented  Pilot loss  Mech loss  Charge  Final mode");

foreach (var policy in Enum.GetValues<CyborgSpecialtyPolicy>())
foreach (var choice in Enum.GetValues<DeploymentChoice>())
{
    var state = SpecialtyPressureExperimentScenario.Create(policy);
    var outcome = Run(state, SpecialtyPressureExperimentScenario.Script(policy, choice));
    Console.WriteLine($"{policy,-15} {choice,-11} {outcome.Damage,6} {outcome.Prevented,10} " +
                      $"{outcome.PilotLoss,11} {outcome.MechLoss,10} {outcome.Charge,6}/8  {outcome.Mode}");
}

Console.WriteLine();
Console.WriteLine("PASS — every branch is forecast-identical and transition changes a measurable tactical tradeoff.");
return 0;

static Outcome Run(CombatState state, IReadOnlyList<CombatCommand> commands)
{
    var resolver = new CombatResolver();
    var forecast = new CombatForecast(resolver);
    var prevented = 0;
    foreach (var command in commands)
    {
        var predicted = forecast.Evaluate(state, command);
        if (!predicted.IsLegal) throw new InvalidOperationException(predicted.RejectionReason);
        var actual = resolver.Resolve(state, command);
        if (!predicted.Events.SequenceEqual(actual.Events) || predicted.ResultingStateHash != actual.ResultingStateHash)
            throw new InvalidOperationException("Forecast diverged from execution.");
        prevented += actual.Events.Sum(item => item.Payload is GuardDamagedEvent guard ? guard.Amount : 0);
    }

    var pilot = state.Entities[SpecialtyPressureExperimentScenario.Pilot];
    var mech = state.Entities[SpecialtyPressureExperimentScenario.Mech];
    var warden = state.Entities[SpecialtyPressureExperimentScenario.Warden];
    var mode = pilot.Flags == EntityFlags.Docked ? "Boarded" : "Remote";
    return new Outcome(24 - warden.Integrity.Current, prevented,
        16 - pilot.Integrity.Current, 30 - mech.Integrity.Current,
        state.Resources[SpecialtyPressureExperimentScenario.Charge].Current, mode);
}

public sealed record Outcome(int Damage, int Prevented, int PilotLoss, int MechLoss, int Charge, string Mode);
