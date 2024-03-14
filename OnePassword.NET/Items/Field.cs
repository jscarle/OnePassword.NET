using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using OnePassword.Common;

namespace OnePassword.Items;

/// <summary>Represents a 1Password item field.</summary>
public sealed class Field : ITracked
{
    /// <summary>The field section.</summary>
    [JsonInclude]
    [JsonPropertyName("section")]
    public Section? Section { get; internal init; }

    /// <summary>The field ID.</summary>
    [JsonInclude]
    [JsonPropertyName("id")]
    public string Id { get; internal init; } = "";

    /// <summary>The field label.</summary>
    [JsonInclude]
    [JsonPropertyName("label")]
    public string Label { get; internal init; } = "";

    /// <summary>The field type.</summary>
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

    /// <summary>The field purpose.</summary>
    [JsonInclude]
    [JsonPropertyName("purpose")]
    public FieldPurpose? Purpose { get; internal init; }

    /// <summary>The field value.</summary>
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

    /// <summary>Password details when the field is a password type field.</summary>
    [JsonInclude]
    [JsonPropertyName("password_details")]
    public PasswordDetails? PasswordDetails { get; internal init; }

    /// <summary>The reference path to the field.</summary>
    [JsonInclude]
    [JsonPropertyName("reference")]
    public string? Reference { get; internal init; }

    /// <summary>Returns <see langword="true" /> when the field type has changed, <see langword="false" /> otherwise.</summary>
    internal bool TypeChanged { get; private set; }

    /// <summary>Returns <see langword="true" /> when the field value has changed, <see langword="false" /> otherwise.</summary>
    internal bool ValueChanged { get; private set; }

    /// <inheritdoc />
    bool ITracked.Changed =>
        TypeChanged || ValueChanged;

    private FieldType _type = FieldType.Unknown;
    private string _value = "";

    /// <summary>Initializes a new instance of <see cref="Field" />.</summary>
    public Field()
    {
    }

    /// <summary>Initializes a new instance of <see cref="Field" /> with the specified label, type, value, and optionally, section.</summary>
    /// <param name="label">The field label.</param>
    /// <param name="type">The field type.</param>
    /// <param name="value">The field value.</param>
    /// <param name="section">The field section.</param>
    [SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase")]
    public Field(string label, FieldType type, string value, Section? section = null)
    {
        Id = label?.ToLower(CultureInfo.InvariantCulture)?.Replace(" ", "_", StringComparison.InvariantCulture) ?? "";
        Label = label ?? "";
        Type = type;
        Value = value;
        Section = section;
    }

    /// <inheritdoc />
    void ITracked.AcceptChanges()
    {
        ValueChanged = false;
    }
}
