namespace SecretGame.Simulation;

public sealed class CommandRejectedException : InvalidOperationException
{
    public CommandRejectedException(string message) : base(message) { }
}
