using System;
using System.Collections.Generic;
using System.Linq;
using SecretGame.Simulation;

var expectedHashes = new Dictionary<string, string>(StringComparer.Ordinal);
foreach (var candidate in new[] { (Name: "1x1", Footprint: new Footprint(1, 1)), (Name: "2x2", Footprint: new Footprint(2, 2)) })
{
    var first = Replay(candidate.Footprint);
    var second = Replay(candidate.Footprint);
    if (first != second) throw new InvalidOperationException($"{candidate.Name} replay diverged locally.");
    expectedHashes[candidate.Name] = first;
    Console.WriteLine($"REPLAY {candidate.Name} {first}");
}

Console.WriteLine();
Console.WriteLine("LEGIBILITY TEST — do not inspect source while answering.");
Console.WriteLine("Predict windows using +Name:Condition to open and -Name:Condition to close.");
Console.WriteLine("Separate multiple answers with commas. Type none if no window changes.");

var state = RelayYardScenario.Create(new Footprint(2, 2));
var commands = RelayYardScenario.ScriptedCommands();
var resolver = new CombatResolver();
var forecast = new CombatForecast(resolver);
var renderer = new TextForecastRenderer();
var testedSteps = new HashSet<int> { 1, 2, 5, 8, 12 };
var correct = 0;
var total = 0;
for (var index = 0; index < commands.Count; index++)
{
    var prediction = forecast.Evaluate(state, commands[index]);
    if (!prediction.IsLegal) throw new InvalidOperationException(prediction.RejectionReason);
    if (testedSteps.Contains(index))
    {
        total++;
        Console.WriteLine();
        Console.WriteLine(renderer.RenderState(state));
        Console.WriteLine($"QUESTION {total}/5 — {Describe(commands[index])}");
        Console.Write("> ");
        var answer = Console.ReadLine() ?? string.Empty;
        var expected = prediction.ConditionsOpened.Select(signal => $"+{Name(signal)}")
            .Concat(prediction.ConditionsClosed.Select(signal => $"-{Name(signal)}"))
            .OrderBy(value => value, StringComparer.OrdinalIgnoreCase)
            .ToArray();
        var supplied = answer.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(value => !value.Equals("none", StringComparison.OrdinalIgnoreCase))
            .OrderBy(value => value, StringComparer.OrdinalIgnoreCase)
            .ToArray();
        var match = expected.SequenceEqual(supplied, StringComparer.OrdinalIgnoreCase);
        if (match) correct++;
        Console.WriteLine($"{(match ? "CORRECT" : "MISS")} — expected {(expected.Length == 0 ? "none" : string.Join(", ", expected))}");
    }
    resolver.Resolve(state, commands[index]);
}

Console.WriteLine();
Console.WriteLine($"LEGIBILITY {correct}/5 {(correct >= 4 ? "PASS" : "FAIL")}");
return correct >= 4 ? 0 : 1;

static string Replay(Footprint footprint)
{
    var replay = new CombatReplay(new CombatResolver()).Run(
        RelayYardScenario.Create(footprint),
        RelayYardScenario.ScriptedCommands());
    return replay.FinalStateHash;
}

static string Name(ConditionSignal signal) => $"{RelayYardScenario.NameOf(signal.EntityId)}:{signal.Kind}";

static string Describe(CombatCommand command) => command switch
{
    MoveCommand move => $"Move {RelayYardScenario.NameOf(move.EntityId)} to ({move.Destination.X},{move.Destination.Y})",
    ApplyConditionCommand condition => $"{RelayYardScenario.NameOf(condition.SourceId)} applies {condition.Kind} to {RelayYardScenario.NameOf(condition.TargetId)}",
    SetDeploymentModeCommand deployment => $"Set {RelayYardScenario.NameOf(deployment.PilotId)} / {RelayYardScenario.NameOf(deployment.MechId)} to {deployment.Mode}",
    _ => command.ToString() ?? command.GetType().Name
};
