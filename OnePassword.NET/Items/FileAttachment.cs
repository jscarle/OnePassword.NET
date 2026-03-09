using OnePassword.Common;

namespace OnePassword.Items;

/// <summary>Represents a file attachment associated with an item.</summary>
public sealed class FileAttachment : ITracked
{
    /// <summary>The section where the attachment is stored.</summary>
    [JsonInclude]
    [JsonPropertyName("section")]
    public Section? Section { get; internal set; }

    /// <summary>The attachment ID.</summary>
    [JsonInclude]
    [JsonPropertyName("id")]
    public string Id { get; internal set; } = "";

    /// <summary>The attachment name.</summary>
    [JsonInclude]
    [JsonPropertyName("name")]
    public string Name { get; internal set; } = "";

    /// <summary>The attachment size in bytes.</summary>
    [JsonInclude]
    [JsonPropertyName("size")]
    public int Size { get; internal set; }

    /// <summary>The path used to access the attachment content.</summary>
    [JsonInclude]
    [JsonPropertyName("content_path")]
    public string ContentPath { get; internal set; } = "";

    /// <inheritdoc />
    public bool Changed => false;

    /// <summary>Initializes a new instance of <see cref="FileAttachment" />.</summary>
    public FileAttachment()
    {
    }

    /// <summary>Initializes a new instance of <see cref="FileAttachment" /> to attach a local file when creating or editing an item.</summary>
    /// <param name="filePath">The local file path to attach.</param>
    /// <param name="name">The attachment name. Leave empty to preserve the source file name.</param>
    /// <param name="section">The section where the attachment should be added.</param>
    public FileAttachment(string filePath, string? name = null, Section? section = null)
    {
        ContentPath = filePath ?? "";
        Name = name ?? "";
        Section = section;
    }

    /// <inheritdoc />
    void ITracked.AcceptChanges()
    {
    }
}
