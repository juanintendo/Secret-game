using System;
using SecretGame.Simulation;
using UnityEditor;
using UnityEngine;

namespace SecretGame.Editor;

public static class RelayYardReplayVerifier
{
    public const string ExpectedOneByOne = "716BE2E159FBB184B422C33910A6A0513FEF8B034D53E531F76E8A2BAACAF0B4";
    public const string ExpectedTwoByTwo = "533CEDD457740A8604C19265EFEF864E361325152A10777E32C1C72D62260689";

    [MenuItem("Secret Game/Verify/Gate A Replay Hashes")]
    public static void VerifyFromMenu()
    {
        VerifyOrThrow();
        Debug.Log("Secret Game Gate A replay hashes match inside Unity.");
    }

    public static void VerifyOrThrow()
    {
        Verify(new Footprint(1, 1), ExpectedOneByOne, "1x1");
        Verify(new Footprint(2, 2), ExpectedTwoByTwo, "2x2");
    }

    private static void Verify(Footprint footprint, string expected, string label)
    {
        var replay = new CombatReplay(new CombatResolver()).Run(
            RelayYardScenario.Create(footprint),
            RelayYardScenario.ScriptedCommands());
        if (!string.Equals(expected, replay.FinalStateHash, StringComparison.Ordinal))
            throw new InvalidOperationException(
                $"Unity replay {label} diverged. Expected {expected}; actual {replay.FinalStateHash}.");
    }
}
