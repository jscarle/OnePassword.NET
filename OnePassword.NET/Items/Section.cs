using System.Globalization;

namespace OnePassword.Items;

public sealed class Section
{
    [JsonInclude]
    [JsonPropertyName("id")]
    public string Id { get; internal init; } = "";

    [JsonInclude]
    [JsonPropertyName("label")]
    public string Label { get; internal init; } = "";

    public Section()
    {
    }

    public Section(string label)
    {
        Id = label.ToLower(CultureInfo.InvariantCulture).Replace(" ", "_", StringComparison.InvariantCulture);
        Label = label;
    }
}