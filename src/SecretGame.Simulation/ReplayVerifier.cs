namespace SecretGame.Simulation;

public static class ReplayVerifier
{
    public static int? FindFirstDivergence(
        IReadOnlyList<ResolvedEvent> expected,
        IReadOnlyList<ResolvedEvent> actual)
    {
        var sharedCount = Math.Min(expected.Count, actual.Count);
        for (var index = 0; index < sharedCount; index++)
        {
            if (expected[index] != actual[index]) return index;
        }

        return expected.Count == actual.Count ? null : sharedCount;
    }
}
