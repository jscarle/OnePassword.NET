using OnePassword.Common;

namespace OnePassword.Items;

/// <summary>
/// Common base class that represents a 1Password item.
/// </summary>
public abstract class ItemBase : ITracked
{
    private string _categoryId = "";
    private bool _categoryIdChanged;
    private string _title = "";

    /// <summary>
    /// The item title.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("title")]
    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            TitleChanged = true;
        }
    }

    /// <summary>
    /// The item categoryId.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("category_id")]
    public string CategoryId
    {
        get => _categoryId;
        set
        {
            _categoryId = value;
            _categoryIdChanged = true;
        }
    }

    /// <summary>
    /// The item category.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("category")]
    public Category Category { get; internal set; } = Category.Unknown;

    /// <summary>
    /// The item sections.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("sections")]
    public TrackedList<Section> Sections { get; internal set; } = [];

    /// <summary>
    /// The item fields.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("fields")]
    public TrackedList<Field> Fields { get; internal set; } = [];

    /// <summary>
    /// The item URLs.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("urls")]
    public TrackedList<Url> Urls { get; internal set; } = [];

    /// <summary>
    /// The tags associated with the item.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("tags")]
    public TrackedList<string> Tags { get; internal set; } = [];

    /// <summary>
    /// Returns <see langword="true" /> when the title has changed, <see langword="false" /> otherwise.
    /// </summary>
    internal bool TitleChanged { get; private set; }

    /// <inheritdoc />
    bool ITracked.Changed => TitleChanged
        || _categoryIdChanged
        || ((ITracked)Sections).Changed
        || ((ITracked)Fields).Changed
        || ((ITracked)Urls).Changed
        || ((ITracked)Tags).Changed;

    /// <inheritdoc />
    void ITracked.AcceptChanges()
    {
        TitleChanged = false;
        _categoryIdChanged = false;
        ((ITracked)Sections).AcceptChanges();
        ((ITracked)Fields).AcceptChanges();
        ((ITracked)Urls).AcceptChanges();
        ((ITracked)Tags).AcceptChanges();
    }
}