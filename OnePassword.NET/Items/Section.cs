using System.Globalization;

namespace OnePassword.Items;

/// <summary>
/// Represents a 1Password item section.
/// </summary>
public sealed class Section
{
    /// <summary>
    /// The section ID.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("id")]
    public string Id { get; internal init; } = "";

    /// <summary>
    /// The section label.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("label")]
    public string Label { get; internal init; } = "";

    /// <summary>
    /// Initializes a new instance of <see cref="Section"/>.
    /// </summary>
    public Section()
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="Section"/> with the specified label.
    /// </summary>
    /// <param name="label">The section label.</param>
    public Section(string label)
    {
        Id = label.ToLower(CultureInfo.InvariantCulture).Replace(" ", "_", StringComparison.InvariantCulture);
        Label = label;
    }
}