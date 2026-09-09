using SecretGame.Simulation;

var state = CyborgMechExperimentScenario.Create();
var resolver = new CombatResolver();
var forecast = new CombatForecast(resolver);
var renderer = new TextForecastRenderer();

Console.WriteLine("CYBORG / MECH RESOURCE EXPERIMENT — RECOMMENDATION, NOT CANON");
Console.WriteLine("Prototype inputs: Charge 3/8 | Recharge +3 for 1 AP | Deploy Remotely -2 for 1 AP");
Console.WriteLine("Remote rule under test: pilot is the initiative owner; mech remains spatial but has no free turn.");
PrintState(state);
Console.WriteLine(renderer.RenderTeachingState(state));

foreach (var command in CyborgMechExperimentScenario.ScriptedCommands())
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
    if (command is BoardMechCommand or DeployMechRemotelyCommand)
        Console.WriteLine(renderer.RenderTeachingState(state));
}

Console.WriteLine();
Console.WriteLine("PASS — one shared pool, exact forecast parity, no free recharge, no second remote initiative slot.");
return 0;

static void PrintState(CombatState state)
{
    var pilot = state.Entities[CyborgMechExperimentScenario.Pilot];
    var mech = state.Entities[CyborgMechExperimentScenario.Mech];
    var charge = state.Resources[CyborgMechExperimentScenario.Charge];
    Console.WriteLine($"STATE Charge {charge.Current}/{charge.Maximum} | Pilot {Flags(pilot.Flags)} AP {pilot.ActionPoints} | Mech {Flags(mech.Flags)} AP {mech.ActionPoints}");
}

static string Flags(EntityFlags flags)
{
    if (flags == EntityFlags.Docked) return "aboard/non-spatial";
    if (flags == EntityFlags.RemoteControlled) return "remote/spatial/no initiative";
    return "deployed/spatial/initiative";
}

static string Describe(CombatCommand command) => command switch
{
    BeginActivationCommand begin => $"Begin activation {RelayYardScenario.NameOf(begin.EntityId)}",
    EndActivationCommand end => $"End activation {RelayYardScenario.NameOf(end.EntityId)}",
    BoardMechCommand => "Board Mech (1 AP; Charge does not change)",
    RechargeMechCommand recharge => $"Recharge while boarded (1 AP; up to +{recharge.Amount} Charge)",
    DeployMechRemotelyCommand deploy => $"Deploy Remotely to ({deploy.PilotDestination.X},{deploy.PilotDestination.Y}) (1 AP; -{deploy.ResourceCost} Charge)",
    _ => command.GetType().Name
};
