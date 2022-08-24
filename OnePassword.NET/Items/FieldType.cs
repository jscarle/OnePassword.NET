using OnePassword.Common;

namespace OnePassword.Items;

[JsonConverter(typeof(JsonStringEnumConverterEx<FieldType>))]
public enum FieldType
{
    [EnumMember(Value = "ADDRESS")]
    Address,

    [EnumMember(Value = "CONCEALED")]
    Concealed,

    [EnumMember(Value = "CREDIT_CARD_NUMBER")]
    CreditCardNumber,

    [EnumMember(Value = "CREDIT_CARD_TYPE")]
    CreditCardType,

    [EnumMember(Value = "DATE")]
    Date,

    [EnumMember(Value = "EMAIL")]
    Email,

    [EnumMember(Value = "FILE")]
    File,

    [EnumMember(Value = "GENDER")]
    Gender,

    [EnumMember(Value = "MENU")]
    Menu,

    [EnumMember(Value = "MONTH_YEAR")]
    MonthYear,

    [EnumMember(Value = "OTP")]
    Otp,

    [EnumMember(Value = "PHONE")]
    Phone,

    [EnumMember(Value = "REFERENCE")]
    Reference,

    [EnumMember(Value = "SSHKEY")]
    Sshkey,

    [EnumMember(Value = "STRING")]
    String,

    Unknown,

    [EnumMember(Value = "URL")]
    Url
}