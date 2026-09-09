using SecretGame.Simulation;

var state = RemoteDirectiveExperimentScenario.Create();
var resolver = new CombatResolver();
var forecast = new CombatForecast(resolver);
var renderer = new TextForecastRenderer();

Console.WriteLine("REMOTE DIRECTIVE EXPERIMENT — OPTION A / RECOMMENDATION, NOT CANON");
Console.WriteLine("Rule under test: each mech directive spends Cyborg AP + shared Charge immediately.");
Console.WriteLine("The remote mech occupies cells and performs events, but owns neither AP nor an initiative slot.");
PrintState(state);
Console.WriteLine(renderer.RenderTeachingState(state));

foreach (var command in RemoteDirectiveExperimentScenario.ScriptedCommands())
{
    Console.WriteLine();
    Console.WriteLine($"> {Describe(command)}");
    var predicted = forecast.Evaluate(state, command);
    Console.WriteLine(renderer.RenderForecast(predicted));
    if (!predicted.IsLegal) return 1;
    var actual = resolver.Resolve(state, command);
    if (!predicted.Events.SequenceEqual(actual.Events) || predicted.ResultingStateHash != actual.ResultingStateHash)
        throw new InvalidOperationException("Forecast diverged from authoritative execution.");
    PrintState(state);
    if (command is RemoteMoveDirectiveCommand)
        Console.WriteLine(renderer.RenderTeachingState(state));
}

Console.WriteLine();
Console.WriteLine("PASS — two mech actions consumed the Cyborg's two AP and two Charge; mech AP stayed untouched.");
return 0;

static void PrintState(CombatState state)
{
    var pilot = state.Entities[RemoteDirectiveExperimentScenario.Pilot];
    var mech = state.Entities[RemoteDirectiveExperimentScenario.Mech];
    var target = state.Entities[RemoteDirectiveExperimentScenario.Target];
    var charge = state.Resources[RemoteDirectiveExperimentScenario.Charge];
    Console.WriteLine($"STATE Charge {charge.Current}/{charge.Maximum} | Cyborg AP {pilot.ActionPoints} | Mech AP {mech.ActionPoints} (no initiative) | Target Integrity {target.Integrity.Current}/{target.Integrity.Maximum}");
}

static string Describe(CombatCommand command) => command switch
{
    BeginActivationCommand => "Begin Cyborg activation",
    RemoteMoveDirectiveCommand move => $"Order Mech move to ({move.Destination.X},{move.Destination.Y}) — 1 Cyborg AP, 1 Charge",
    RemoteAttackDirectiveCommand attack => $"Order Mech attack Target for {attack.Damage} — 1 Cyborg AP, 1 Charge",
    EndActivationCommand => "End Cyborg activation",
    _ => command.GetType().Name
};
