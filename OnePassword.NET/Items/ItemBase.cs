using OnePassword.Common;

namespace OnePassword.Items;

/// <summary>
/// Common base class that represents a 1Password item.
/// </summary>
public abstract class ItemBase : ITracked
{
    /// <summary>
    /// The item title.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("title")]
    public string Title
    {
        get => _title;
        set {
            _title = value;
            TitleChanged = true;
        }
    }

    /// <summary>
    /// The item category.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("category")]
    public Category Category { get; internal init; } = Category.Unknown;

    /// <summary>
    /// The item sections.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("sections")]
    public TrackedList<Section> Sections { get; internal init; } = new();

    /// <summary>
    /// The item fields.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("fields")]
    public TrackedList<Field> Fields { get; internal init; } = new();

    /// <summary>
    /// The item URLs.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("urls")]
    public TrackedList<Url> Urls { get; internal init; } = new();

    /// <summary>
    /// The tags associated with the item.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("tags")]
    public TrackedList<string> Tags { get; internal init; } = new();

    /// <summary>
    /// Returns <see langword="true"/> when the title has changed, <see langword="false"/> otherwise.
    /// </summary>
    internal bool TitleChanged { get; private set; }

    /// <inheritdoc />
    bool ITracked.Changed => TitleChanged
        | ((ITracked)Sections).Changed
        | ((ITracked)Fields).Changed
        | ((ITracked)Urls).Changed
        | ((ITracked)Tags).Changed;

    private string _title = "";

    /// <inheritdoc />
    void ITracked.AcceptChanges()
    {
        TitleChanged = false;
        ((ITracked)Sections).AcceptChanges();
        ((ITracked)Fields).AcceptChanges();
        ((ITracked)Urls).AcceptChanges();
        ((ITracked)Tags).AcceptChanges();
    }
}