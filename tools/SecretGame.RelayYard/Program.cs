using System;
using SecretGame.Simulation;

var state = RelayYardScenario.Create();
var resolver = new CombatResolver();
var forecast = new CombatForecast(resolver);
var renderer = new TextForecastRenderer();

Console.WriteLine("RELAY YARD — HEADLESS FORECAST HARNESS");
Console.WriteLine("H Human | C Cyborg | M Mech | S Synthetic | E Enemy | # Blocked");
Console.WriteLine(renderer.RenderState(state));

var step = 1;
foreach (var command in RelayYardScenario.ScriptedCommands())
{
    Console.WriteLine($"STEP {step}: {command}");
    var prediction = forecast.Evaluate(state, command);
    Console.WriteLine(renderer.RenderForecast(prediction));
    if (!prediction.IsLegal) return 1;
    var result = resolver.Resolve(state, command);
    if (result.ResultingStateHash != prediction.ResultingStateHash) return 2;
    Console.WriteLine();
    step++;
}

Console.WriteLine(renderer.RenderState(state));
Console.WriteLine($"COMPLETE {state.Events.Count} events | {state.DeterministicHash()}");
return 0;
