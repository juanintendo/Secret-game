using System;
using System.Linq;
using SecretGame.Simulation;

var one = Run(new Footprint(1, 1));
var two = Run(new Footprint(2, 2));
Console.WriteLine($"1x1 legal={one.Legal} events={one.EventCount} mech-steps={one.MechSteps} hash={one.Hash}");
Console.WriteLine($"2x2 legal={two.Legal} events={two.EventCount} mech-steps={two.MechSteps} hash={two.Hash}");
Console.WriteLine($"rule-stream-shape-equal={one.EventTypes.SequenceEqual(two.EventTypes)}");
return one.Legal && two.Legal && one.EventTypes.SequenceEqual(two.EventTypes) ? 0 : 1;

static Result Run(Footprint footprint)
{
    var state = RelayYardScenario.Create(footprint);
    var resolver = new CombatResolver();
    var forecast = new CombatForecast(resolver);
    foreach (var command in RelayYardScenario.ScriptedCommands())
    {
        var predicted = forecast.Evaluate(state, command);
        if (!predicted.IsLegal) return new Result(false, state.Events.Count, 0, state.DeterministicHash(), Array.Empty<string>());
        var actual = resolver.Resolve(state, command);
        if (predicted.ResultingStateHash != actual.ResultingStateHash)
            return new Result(false, state.Events.Count, 0, state.DeterministicHash(), Array.Empty<string>());
    }
    var move = state.Events.Select(item => item.Payload).OfType<EntityMovedEvent>()
        .Single(item => item.EntityId == RelayYardScenario.Mech);
    return new Result(true, state.Events.Count, move.Path.StepCount, state.DeterministicHash(),
        state.Events.Select(item => item.Payload.GetType().Name).ToArray());
}

sealed record Result(bool Legal, int EventCount, int MechSteps, string Hash, string[] EventTypes);
