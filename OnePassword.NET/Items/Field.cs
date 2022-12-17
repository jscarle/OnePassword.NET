using System.Globalization;
using OnePassword.Common;

namespace OnePassword.Items;

public sealed class Field : ITracked
{
    [JsonInclude]
    [JsonPropertyName("section")]
    public Section? Section { get; internal init; }

    [JsonInclude]
    [JsonPropertyName("id")]
    public string Id { get; internal init; } = "";

    [JsonInclude]
    [JsonPropertyName("label")]
    public string Label { get; internal init; } = "";

    [JsonInclude]
    [JsonPropertyName("type")]
    public FieldType Type
    {
        get => _type;
        set
        {
            _type = value;
            TypeChanged = true;
        }
    }

    [JsonInclude]
    [JsonPropertyName("purpose")]
    public FieldPurpose? Purpose { get; internal init; }

    [JsonInclude]
    [JsonPropertyName("value")]
    public string Value
    {
        get => _value;
        set
        {
            _value = value;
            ValueChanged = true;
        }
    }

    [JsonInclude]
    [JsonPropertyName("password_details")]
    public PasswordDetails? PasswordDetails { get; internal init; }

    [JsonInclude]
    [JsonPropertyName("reference")]
    public string? Reference { get; internal init; }

    internal bool TypeChanged { get; private set; }

    internal bool ValueChanged { get; private set; }

    bool ITracked.Changed =>
        TypeChanged
        | ValueChanged;

    private FieldType _type = FieldType.Unknown;
    private string _value = "";

    void ITracked.AcceptChanges()
    {
        ValueChanged = false;
    }

    public Field()
    {
    }

    public Field(string label, FieldType type,  string value, Section? section = null)
    {
        Id = label.ToLower(CultureInfo.InvariantCulture).Replace(" ", "_", StringComparison.InvariantCulture);
        Label = label;
        Type = type;
        Value = value;
        Section = section;
    }
}