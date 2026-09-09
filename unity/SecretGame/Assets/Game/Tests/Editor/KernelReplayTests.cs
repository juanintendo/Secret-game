using NUnit.Framework;
using SecretGame.Editor;
using SecretGame.Presentation;
using SecretGame.Simulation;

namespace SecretGame.Unity.EditorTests;

public sealed class KernelReplayTests
{
    [Test]
    public void RelayYardHashesMatchGateAInsideUnity() =>
        RelayYardReplayVerifier.VerifyOrThrow();

    [Test]
    public void SimulationAssemblyHasNoUnityEngineReference()
    {
        var references = typeof(CombatState).Assembly.GetReferencedAssemblies();
        Assert.That(references, Has.None.Matches<System.Reflection.AssemblyName>(
            reference => reference.Name is not null && reference.Name.StartsWith("UnityEngine")));
    }

    [Test]
    public void PresentationInboxAcceptsEventsWithoutCombatState()
    {
        var methods = typeof(CombatEventInbox).GetMethods();
        Assert.That(methods, Has.None.Matches<System.Reflection.MethodInfo>(method =>
            method.ReturnType == typeof(CombatState) ||
            System.Array.Exists(method.GetParameters(), parameter => parameter.ParameterType == typeof(CombatState))));
    }

    [Test]
    public void CyborgMechResourceForecastMatchesExecution()
    {
        var state = CyborgMechExperimentScenario.Create();
        var resolver = new CombatResolver();
        var forecast = new CombatForecast(resolver);
        foreach (var command in CyborgMechExperimentScenario.ScriptedCommands())
        {
            var predicted = forecast.Evaluate(state, command);
            Assert.That(predicted.IsLegal, Is.True, predicted.RejectionReason);
            var actual = resolver.Resolve(state, command);
            Assert.That(actual.Events, Is.EqualTo(predicted.Events));
            Assert.That(actual.ResultingStateHash, Is.EqualTo(predicted.ResultingStateHash));
        }

        Assert.That(state.Resources[CyborgMechExperimentScenario.Charge].Current, Is.EqualTo(4));
        Assert.That(state.Entities[CyborgMechExperimentScenario.Pilot].Flags, Is.EqualTo(EntityFlags.Active));
        Assert.That(state.Entities[CyborgMechExperimentScenario.Mech].Flags, Is.EqualTo(EntityFlags.RemoteControlled));
    }

    [Test]
    public void RemoteDirectiveForecastMatchesExecutionWithoutMechActivation()
    {
        var state = RemoteDirectiveExperimentScenario.Create();
        var resolver = new CombatResolver();
        var forecast = new CombatForecast(resolver);
        foreach (var command in RemoteDirectiveExperimentScenario.ScriptedCommands())
        {
            var predicted = forecast.Evaluate(state, command);
            Assert.That(predicted.IsLegal, Is.True, predicted.RejectionReason);
            var actual = resolver.Resolve(state, command);
            Assert.That(actual.Events, Is.EqualTo(predicted.Events));
            Assert.That(actual.ResultingStateHash, Is.EqualTo(predicted.ResultingStateHash));
        }

        Assert.That(state.Resources[RemoteDirectiveExperimentScenario.Charge].Current, Is.EqualTo(2));
        Assert.That(state.Entities[RemoteDirectiveExperimentScenario.Pilot].ActionPoints, Is.EqualTo(0));
        Assert.That(state.Entities[RemoteDirectiveExperimentScenario.Mech].ActionPoints, Is.EqualTo(2));
        Assert.That(state.Entities[RemoteDirectiveExperimentScenario.Target].Integrity.Current, Is.EqualTo(12));
    }
}
