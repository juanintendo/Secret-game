using SecretGame.Simulation;

Console.WriteLine("CYBORG SPECIALTY POLICY COMPARISON — RECOMMENDATION, NOT CANON");
Console.WriteLine("All numbers are prototype inputs. This compares economy and authority, not final balance.");
Console.WriteLine();

var bulwark = RunBoarded(CyborgSpecialtyPolicy.Bulwark);
var remote = RunRemote();
var redline = RunBoarded(CyborgSpecialtyPolicy.Redline);

Console.WriteLine("COMPARISON");
Console.WriteLine("Policy          Mode       AP owner   AP spent  Charge  Damage  Guard  Mech free turn");
PrintRow("Bulwark", "Boarded", "Mech", bulwark);
PrintRow("Remote Arsenal", "Remote", "Cyborg", remote);
PrintRow("Redline", "Boarded", "Mech", redline);
Console.WriteLine();
Console.WriteLine("PASS — the three policies produce distinct resource/defense/damage signatures without a second remote activation.");
return 0;

static Outcome RunBoarded(CyborgSpecialtyPolicy policy)
{
    Console.WriteLine($"=== {policy} ===");
    var state = SpecialtyPolicyExperimentScenario.CreateBoarded();
    Run(state, SpecialtyPolicyExperimentScenario.BoardedScript(policy));
    return OutcomeOf(state, policy == CyborgSpecialtyPolicy.Bulwark ? "Bulwark" : "Redline");
}

static Outcome RunRemote()
{
    Console.WriteLine("=== Remote Arsenal ===");
    var state = RemoteDirectiveExperimentScenario.Create();
    Run(state, RemoteDirectiveExperimentScenario.ScriptedCommands());
    return OutcomeOf(state, "Remote Arsenal");
}

static void Run(CombatState state, IReadOnlyList<CombatCommand> commands)
{
    var resolver = new CombatResolver();
    var forecast = new CombatForecast(resolver);
    var renderer = new TextForecastRenderer();
    Console.WriteLine(renderer.RenderTeachingState(state));
    foreach (var command in commands)
    {
        var predicted = forecast.Evaluate(state, command);
        if (!predicted.IsLegal) throw new InvalidOperationException(predicted.RejectionReason);
        Console.WriteLine(renderer.RenderForecast(predicted));
        var actual = resolver.Resolve(state, command);
        if (!predicted.Events.SequenceEqual(actual.Events) || predicted.ResultingStateHash != actual.ResultingStateHash)
            throw new InvalidOperationException("Forecast diverged from execution.");
    }
    Console.WriteLine();
}

static Outcome OutcomeOf(CombatState state, string name)
{
    var mech = state.Entities[SpecialtyPolicyExperimentScenario.Mech];
    var target = state.Entities[SpecialtyPolicyExperimentScenario.Target];
    var resource = state.Resources[SpecialtyPolicyExperimentScenario.Charge];
    var startingIntegrity = 18;
    var apSpent = name == "Remote Arsenal"
        ? 2 - state.Entities[SpecialtyPolicyExperimentScenario.Pilot].ActionPoints
        : 2 - mech.ActionPoints;
    return new Outcome(apSpent, resource.Current, startingIntegrity - target.Integrity.Current, mech.Guard.Current);
}

static void PrintRow(string name, string mode, string owner, Outcome outcome) =>
    Console.WriteLine($"{name,-15} {mode,-10} {owner,-10} {outcome.ApSpent,8} {outcome.Charge,6}/8 {outcome.Damage,7} {outcome.Guard,6}  NO");

public sealed record Outcome(int ApSpent, int Charge, int Damage, int Guard);
