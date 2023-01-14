using OnePassword.Common;

namespace OnePassword.Items;

/// <summary>
/// Represents a 1Password item URL.
/// </summary>
public sealed class Url : ITracked
{
    /// <summary>
    /// The URL label.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("label")]
    public string Label { get; internal init; } = "";

    /// <summary>
    /// The URL HREF.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("href")]
    public string Href
    {
        get => _href;
        set
        {
            _href = value;
            _changed = true;
        }
    }

    /// <summary>
    /// Returns <see langword="true"/> when the URL is the primary URL for the item, <see langword="false"/> otherwise.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("primary")]
    public bool Primary { get; internal init; }

    /// <inheritdoc />
    bool ITracked.Changed => _changed;

    private string _href = "";
    private bool _changed;

    /// <inheritdoc />
    void ITracked.AcceptChanges()
    {
        _changed = false;
    }
}