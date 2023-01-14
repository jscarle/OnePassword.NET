using OnePassword.Common;

namespace OnePassword.Items;

/// <summary>
/// Represents the category of a 1Password item.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverterEx<Category>))]
public enum Category
{
    /// <summary>
    /// API Credential
    /// </summary>
    [EnumMember(Value = "API Credential")]
    ApiCredential,

    /// <summary>
    /// Bank Account
    /// </summary>
    [EnumMember(Value = "Bank Account")]
    BankAccount,

    /// <summary>
    /// Credit Card
    /// </summary>
    [EnumMember(Value = "Credit Card")]
    CreditCard,

    /// <summary>
    /// Custom
    /// </summary>
    [EnumMember(Value = "Custom")]
    Custom,

    /// <summary>
    /// Database
    /// </summary>
    [EnumMember(Value = "Database")]
    Database,

    /// <summary>
    /// Document
    /// </summary>
    [EnumMember(Value = "Document")]
    Document,

    /// <summary>
    /// Driver's License
    /// </summary>
    [EnumMember(Value = "Driver License")]
    DriverLicense,

    /// <summary>
    /// Email Account
    /// </summary>
    [EnumMember(Value = "Email Account")]
    EmailAccount,

    /// <summary>
    /// Identity
    /// </summary>
    [EnumMember(Value = "Identity")]
    Identity,

    /// <summary>
    /// Login
    /// </summary>
    [EnumMember(Value = "Login")]
    Login,

    /// <summary>
    /// Medical Record
    /// </summary>
    [EnumMember(Value = "Medical Record")]
    MedicalRecord,

    /// <summary>
    /// Membership
    /// </summary>
    [EnumMember(Value = "Membership")]
    Membership,

    /// <summary>
    /// Outdoor License
    /// </summary>
    [EnumMember(Value = "Outdoor License")]
    OutdoorLicense,

    /// <summary>
    /// Passport
    /// </summary>
    [EnumMember(Value = "Passport")]
    Passport,

    /// <summary>
    /// Password
    /// </summary>
    [EnumMember(Value = "Password")]
    Password,

    /// <summary>
    /// Reward Program
    /// </summary>
    [EnumMember(Value = "Reward Program")]
    RewardProgram,

    /// <summary>
    /// Secure Note
    /// </summary>
    [EnumMember(Value = "Secure Note")]
    SecureNote,

    /// <summary>
    /// Server
    /// </summary>
    [EnumMember(Value = "Server")]
    Server,

    /// <summary>
    /// Social Security Number
    /// </summary>
    [EnumMember(Value = "Social Security Number")]
    SocialSecurityNumber,

    /// <summary>
    /// Software License
    /// </summary>
    [EnumMember(Value = "Software License")]
    SoftwareLicense,

    /// <summary>
    /// SSH Key
    /// </summary>
    [EnumMember(Value = "SSH Key")]
    SshKey,

    /// <summary>
    /// The category is unknown.
    /// </summary>
    Unknown,

    /// <summary>
    /// Wireless Router
    /// </summary>
    [EnumMember(Value = "Wireless Router")]
    WirelessRouter
}