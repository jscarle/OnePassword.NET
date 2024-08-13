using OnePassword.Common;

namespace OnePassword.Items;

/// <summary>Represents a file object.</summary>
public sealed class File : ITracked
{
    /// <summary>The file section.</summary>
    [JsonInclude]
    [JsonPropertyName("section")]
    public Section? Section { get; internal set; }

    /// <summary>The file ID.</summary>
    [JsonInclude]
    [JsonPropertyName("id")]
    public string Id { get; internal set; } = "";

    /// <summary>The name of file.</summary>
    [JsonInclude]
    [JsonPropertyName("name")]
    public string Name { get; internal set; } = "";

    /// <summary>The file size.</summary>
    [JsonInclude]
    [JsonPropertyName("size")]
    public int Size { get; internal set; }

    /// <summary>The content path.</summary>
    [JsonInclude]
    [JsonPropertyName("content_path")]
    public string ContentPath { get; internal set; } = "";

    /// <inheritdoc />
    public bool Changed => false;

    /// <summary>Initializes a new instance of <see cref="File" />.</summary>
    public File()
    {
    }

    /// <inheritdoc />
    void ITracked.AcceptChanges()
    {
    }
}
