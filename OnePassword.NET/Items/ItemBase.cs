using OnePassword.Common;

namespace OnePassword.Items;

public abstract class ItemBase : ITracked
{
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

    [JsonInclude]
    [JsonPropertyName("category")]
    public Category Category { get; internal init; } = Category.Unknown;

    [JsonInclude]
    [JsonPropertyName("sections")]
    public TrackedList<Section> Sections { get; internal init; } = new();

    [JsonInclude]
    [JsonPropertyName("fields")]
    public TrackedList<Field> Fields { get; internal init; } = new();

    [JsonInclude]
    [JsonPropertyName("urls")]
    public TrackedList<Url> Urls { get; internal init; } = new();

    [JsonInclude]
    [JsonPropertyName("tags")]
    public TrackedList<string> Tags { get; internal init; } = new();

    internal bool TitleChanged { get; private set; }

    bool ITracked.Changed => TitleChanged
        | ((ITracked)Sections).Changed
        | ((ITracked)Fields).Changed
        | ((ITracked)Urls).Changed
        | ((ITracked)Tags).Changed;

    private string _title = "";

    void ITracked.AcceptChanges()
    {
        TitleChanged = false;
        ((ITracked)Sections).AcceptChanges();
        ((ITracked)Fields).AcceptChanges();
        ((ITracked)Urls).AcceptChanges();
        ((ITracked)Tags).AcceptChanges();
    }
}