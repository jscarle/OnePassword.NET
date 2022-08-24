namespace OnePassword.Common;

[JsonConverter(typeof(JsonStringEnumConverterEx<Language>))]
public enum Language
{
    [EnumMember(Value = "zh-CHS")]
    ChineseSimplified,

    [EnumMember(Value = "zh-CHT")]
    ChineseTraditional,

    [EnumMember(Value = "nl")]
    Dutch,

    Default,

    [EnumMember(Value = "en")]
    English,

    [EnumMember(Value = "fr")]
    French,

    [EnumMember(Value = "de")]
    German,

    [EnumMember(Value = "it")]
    Italian,

    [EnumMember(Value = "ja")]
    Japanese,

    [EnumMember(Value = "ko")]
    Korean,

    [EnumMember(Value = "pt")]
    Portuguese,

    [EnumMember(Value = "ru")]
    Russian,

    [EnumMember(Value = "es")]
    Spanish,

    Unknown
}
