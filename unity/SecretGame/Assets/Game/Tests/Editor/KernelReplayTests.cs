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
}
