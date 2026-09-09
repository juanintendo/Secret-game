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
Console.WriteLine("LEGIBILITY TEST — informed player test; do not inspect source while answering.");
Console.WriteLine();
Console.WriteLine("WHAT YOU ARE PREDICTING");
Console.WriteLine("A tactical window OPENS when a condition becomes true after the stated action.");
Console.WriteLine("A tactical window CLOSES when a condition that was true stops being true.");
Console.WriteLine("You are NOT choosing a route or describing the animation. The command is already chosen.");
Console.WriteLine();
Console.WriteLine("RULES USED BY THESE FIVE QUESTIONS");
Console.WriteLine("- Coordinates are (X,Y), start at 0 in the upper-left, X goes right, Y goes down.");
Console.WriteLine("- Adjacent includes all eight surrounding cells, including diagonals.");
Console.WriteLine("- Isolated: a spatial unit has no adjacent spatial ally.");
Console.WriteLine("- Marked: the named target has the applied Marked status.");
Console.WriteLine("- Docked: Cyborg still exists, but leaves the map while inside Mech; adjacency updates.");
Console.WriteLine("- A 2x2 Mech occupies all four cells printed M.");
Console.WriteLine();
Console.WriteLine("HOW TO ANSWER");
Console.WriteLine("Choose one numbered description. Type only 1, 2, 3, or 4, then press Enter.");

var state = RelayYardScenario.Create(new Footprint(2, 2));
var commands = RelayYardScenario.ScriptedCommands();
var resolver = new CombatResolver();
var forecast = new CombatForecast(resolver);
var renderer = new TextForecastRenderer();
var testedSteps = new HashSet<int> { 1, 2, 5, 8, 12 };
var choicesByStep = ChoicesByStep();
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
        Console.WriteLine(renderer.RenderTeachingState(state));
        Console.WriteLine($"QUESTION {total}/5 — {Describe(commands[index])}");
        var expected = prediction.ConditionsOpened.Select(signal => $"+{Name(signal)}")
            .Concat(prediction.ConditionsClosed.Select(signal => $"-{Name(signal)}"))
            .OrderBy(value => value, StringComparer.OrdinalIgnoreCase)
            .ToArray();
        var choices = choicesByStep[index];
        for (var choiceIndex = 0; choiceIndex < choices.Count; choiceIndex++)
            Console.WriteLine($"  {choiceIndex + 1}. {choices[choiceIndex].Description}");
        var selected = ReadChoice(choices.Count);
        var supplied = choices[selected].Signals
            .OrderBy(value => value, StringComparer.OrdinalIgnoreCase)
            .ToArray();
        var match = expected.SequenceEqual(supplied, StringComparer.OrdinalIgnoreCase);
        if (match) correct++;
        Console.WriteLine($"{(match ? "CORRECT" : "MISS")} — forecast says {Explain(expected)}");
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

static int ReadChoice(int choiceCount)
{
    while (true)
    {
        Console.Write("> ");
        var answer = Console.ReadLine();
        if (int.TryParse(answer, out var selected) && selected >= 1 && selected <= choiceCount)
            return selected - 1;
        Console.WriteLine($"Please type one number from 1 to {choiceCount}. No command or route description is needed.");
    }
}

static string Explain(IReadOnlyList<string> signals)
{
    if (signals.Count == 0) return "no tactical window changes.";
    return string.Join("; ", signals.Select(signal => signal[0] == '+'
        ? $"{signal[1..]} opens"
        : $"{signal[1..]} closes")) + ".";
}

static IReadOnlyDictionary<int, IReadOnlyList<AnswerChoice>> ChoicesByStep() =>
    new Dictionary<int, IReadOnlyList<AnswerChoice>>
    {
        [1] = new[]
        {
            Choice("No tactical window changes."),
            Choice("Human becomes Isolated.", "+Human:Isolated"),
            Choice("Synthetic becomes Isolated.", "+Synthetic:Isolated"),
            Choice("Warden becomes Marked.", "+Warden:Marked")
        },
        [2] = new[]
        {
            Choice("Warden becomes Marked.", "+Warden:Marked"),
            Choice("Human becomes Marked.", "+Human:Marked"),
            Choice("Warden stops being Marked.", "-Warden:Marked"),
            Choice("No tactical window changes.")
        },
        [5] = new[]
        {
            Choice("Cyborg becomes Isolated.", "+Cyborg:Isolated"),
            Choice("Synthetic becomes Isolated.", "+Synthetic:Isolated"),
            Choice("Mech stops being Isolated.", "-Mech:Isolated"),
            Choice("No tactical window changes.")
        },
        [8] = new[]
        {
            Choice("Only Human stops being Isolated.", "-Human:Isolated"),
            Choice("Only Mech stops being Isolated.", "-Mech:Isolated"),
            Choice("Human and Mech both stop being Isolated.", "-Human:Isolated", "-Mech:Isolated"),
            Choice("No tactical window changes.")
        },
        [12] = new[]
        {
            Choice("Synthetic stops being Isolated.", "-Synthetic:Isolated"),
            Choice("Synthetic becomes Isolated.", "+Synthetic:Isolated"),
            Choice("Human becomes Isolated.", "+Human:Isolated"),
            Choice("No tactical window changes.")
        }
    };

static AnswerChoice Choice(string description, params string[] signals) => new(description, signals);

static string Describe(CombatCommand command) => command switch
{
    MoveCommand move => $"Move {RelayYardScenario.NameOf(move.EntityId)} to ({move.Destination.X},{move.Destination.Y})",
    ApplyConditionCommand condition => $"{RelayYardScenario.NameOf(condition.SourceId)} applies {condition.Kind} to {RelayYardScenario.NameOf(condition.TargetId)}",
    SetDeploymentModeCommand deployment => $"Set {RelayYardScenario.NameOf(deployment.PilotId)} / {RelayYardScenario.NameOf(deployment.MechId)} to {deployment.Mode}",
    _ => command.ToString() ?? command.GetType().Name
};

internal sealed record AnswerChoice(string Description, IReadOnlyList<string> Signals);
