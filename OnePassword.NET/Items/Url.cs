using OnePassword.Common;

namespace OnePassword.Items;

public sealed class Url : ITracked
{
    [JsonInclude]
    [JsonPropertyName("label")]
    public string Label { get; internal init; } = "";

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

    [JsonInclude]
    [JsonPropertyName("primary")]
    public bool Primary { get; internal init; }

    bool ITracked.Changed => _changed;

    private string _href = "";
    private bool _changed;

    void ITracked.AcceptChanges()
    {
        _changed = false;
    }
}