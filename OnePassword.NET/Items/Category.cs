using OnePassword.Common;

namespace OnePassword.Items;

[JsonConverter(typeof(JsonStringEnumConverterEx<Category>))]
public enum Category
{
    [EnumMember(Value = "API Credential")]
    ApiCredential,

    [EnumMember(Value = "Bank Account")]
    BankAccount,

    [EnumMember(Value = "Credit Card")]
    CreditCard,

    [EnumMember(Value = "Custom")]
    Custom,

    [EnumMember(Value = "Database")]
    Database,

    [EnumMember(Value = "Document")]
    Document,

    [EnumMember(Value = "Driver License")]
    DriverLicense,

    [EnumMember(Value = "Email Account")]
    EmailAccount,

    [EnumMember(Value = "Identity")]
    Identity,

    [EnumMember(Value = "Login")]
    Login,

    [EnumMember(Value = "Medical Record")]
    MedicalRecord,

    [EnumMember(Value = "Membership")]
    Membership,

    [EnumMember(Value = "Outdoor License")]
    OutdoorLicense,

    [EnumMember(Value = "Passport")]
    Passport,

    [EnumMember(Value = "Password")]
    Password,

    [EnumMember(Value = "Reward Program")]
    RewardProgram,

    [EnumMember(Value = "Secure Note")]
    SecureNote,

    [EnumMember(Value = "Server")]
    Server,

    [EnumMember(Value = "Social Security Number")]
    SocialSecurityNumber,

    [EnumMember(Value = "Software License")]
    SoftwareLicense,

    [EnumMember(Value = "SSH Key")]
    SshKey,

    Unknown,

    [EnumMember(Value = "Wireless Router")]
    WirelessRouter
}