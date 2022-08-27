using OnePassword.Common;

namespace OnePassword.Items;

[JsonConverter(typeof(JsonStringEnumConverterEx<FieldType>))]
public enum FieldType
{
    [EnumMember(Value = "Address")]
    Address,

    [EnumMember(Value = "Concealed")]
    Concealed,

    [EnumMember(Value = "Credit Card Number")]
    CreditCardNumber,

    [EnumMember(Value = "Credit Card Type")]
    CreditCardType,

    [EnumMember(Value = "Date")]
    Date,

    [EnumMember(Value = "Email")]
    Email,

    [EnumMember(Value = "File")]
    File,

    [EnumMember(Value = "Gender")]
    Gender,

    [EnumMember(Value = "Menu")]
    Menu,

    [EnumMember(Value = "Month Year")]
    MonthYear,

    [EnumMember(Value = "Otp")]
    Otp,

    [EnumMember(Value = "Phone")]
    Phone,

    [EnumMember(Value = "Reference")]
    Reference,

    [EnumMember(Value = "SSHKey")]
    SshKey,

    [EnumMember(Value = "String")]
    String,

    Unknown,

    [EnumMember(Value = "Url")]
    Url
}