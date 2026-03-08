namespace OnePassword.Items;

/// <summary>
/// Represents the result of sharing a 1Password item.
/// </summary>
public sealed class ItemShareResult
{
    /// <summary>
    /// The generated share URL.
    /// </summary>
    public Uri? Url { get; internal set; }

    /// <summary>
    /// The date and time when the share expires, when returned by the CLI.
    /// </summary>
    public DateTimeOffset? ExpiresAt { get; internal set; }

    /// <summary>
    /// The recipients associated with the share, when returned by the CLI.
    /// </summary>
    public ImmutableList<string> Recipients { get; internal set; } = [];

    /// <summary>
    /// Whether the share is view-once, when returned by the CLI.
    /// </summary>
    public bool? ViewOnce { get; internal set; }

    /// <summary>
    /// The raw CLI response used to build the result.
    /// </summary>
    public string RawResponse { get; internal set; } = "";
}
