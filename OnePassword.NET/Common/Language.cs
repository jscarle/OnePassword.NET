namespace OnePassword.Common;

/// <summary>
/// Represents a language.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverterEx<Language>))]
public enum Language
{
    /// <summary>
    /// Chinese (Simplified)
    /// </summary>
    [EnumMember(Value = "zh-CHS")]
    ChineseSimplified,

    /// <summary>
    /// Chinese (Traditional)
    /// </summary>
    [EnumMember(Value = "zh-CHT")]
    ChineseTraditional,

    /// <summary>
    /// Dutch
    /// </summary>
    [EnumMember(Value = "nl")]
    Dutch,

    /// <summary>
    /// The default language.
    /// </summary>
    Default,

    /// <summary>
    /// English
    /// </summary>
    [EnumMember(Value = "en")]
    English,

    /// <summary>
    /// French
    /// </summary>
    [EnumMember(Value = "fr")]
    French,

    /// <summary>
    /// German
    /// </summary>
    [EnumMember(Value = "de")]
    German,

    /// <summary>
    /// Italian
    /// </summary>
    [EnumMember(Value = "it")]
    Italian,

    /// <summary>
    /// Japanese
    /// </summary>
    [EnumMember(Value = "ja")]
    Japanese,

    /// <summary>
    /// Korean
    /// </summary>
    [EnumMember(Value = "ko")]
    Korean,

    /// <summary>
    /// Portuguese
    /// </summary>
    [EnumMember(Value = "pt")]
    Portuguese,

    /// <summary>
    /// Russian
    /// </summary>
    [EnumMember(Value = "ru")]
    Russian,

    /// <summary>
    /// Spanish
    /// </summary>
    [EnumMember(Value = "es")]
    Spanish,

    /// <summary>
    /// The language is unknown.
    /// </summary>
    Unknown
}
