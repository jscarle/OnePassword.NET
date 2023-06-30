using System.Diagnostics.CodeAnalysis;
using OnePassword.Common;

namespace OnePassword.Items;

/// <summary>
/// Represents the type of 1Password field.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverterEx<FieldType>))]
[SuppressMessage("Naming", "CA1720:Identifier contains type name")]
public enum FieldType
{
    /// <summary>
    /// Address
    /// </summary>
    [EnumMember(Value = "Address")]
    Address,

    /// <summary>
    /// Concealed
    /// </summary>
    [EnumMember(Value = "Concealed")]
    Concealed,

    /// <summary>
    /// Credit Card Number
    /// </summary>
    [EnumMember(Value = "Credit Card Number")]
    CreditCardNumber,

    /// <summary>
    /// Credit Card Type
    /// </summary>
    [EnumMember(Value = "Credit Card Type")]
    CreditCardType,

    /// <summary>
    /// Date
    /// </summary>
    [EnumMember(Value = "Date")]
    Date,

    /// <summary>
    /// Email
    /// </summary>
    [EnumMember(Value = "Email")]
    Email,

    /// <summary>
    /// File
    /// </summary>
    [EnumMember(Value = "File")]
    File,

    /// <summary>
    /// Gender
    /// </summary>
    [EnumMember(Value = "Gender")]
    Gender,

    /// <summary>
    /// Menu
    /// </summary>
    [EnumMember(Value = "Menu")]
    Menu,

    /// <summary>
    /// Month Year
    /// </summary>
    [EnumMember(Value = "Month Year")]
    MonthYear,

    /// <summary>
    /// OTP
    /// </summary>
    [EnumMember(Value = "Otp")]
    Otp,

    /// <summary>
    /// Phone
    /// </summary>
    [EnumMember(Value = "Phone")]
    Phone,

    /// <summary>
    /// Reference
    /// </summary>
    [EnumMember(Value = "Reference")]
    Reference,

    /// <summary>
    /// SSH Key
    /// </summary>
    [EnumMember(Value = "SSHKey")]
    SshKey,

    /// <summary>
    /// String
    /// </summary>
    [EnumMember(Value = "String")]
    String,

    /// <summary>
    /// The field type is unknown.
    /// </summary>
    Unknown,

    /// <summary>
    /// Url
    /// </summary>
    [EnumMember(Value = "Url")]
    Url
}