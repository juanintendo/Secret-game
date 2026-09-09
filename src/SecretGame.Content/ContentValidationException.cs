namespace SecretGame.Content;

public sealed class ContentValidationException : InvalidOperationException
{
    public ContentValidationException(string message) : base(message) { }
}
