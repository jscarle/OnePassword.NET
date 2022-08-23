using OnePassword.Common;

namespace OnePassword.Items;

[JsonConverter(typeof(JsonStringEnumConverterEx<Category>))]
public enum Category
{
    [EnumMember(Value = "API_CREDENTIAL")]
    ApiCredential,

    [EnumMember(Value = "BANK_ACCOUNT")]
    BankAccount,

    [EnumMember(Value = "CREDIT_CARD")]
    CreditCard,

    [EnumMember(Value = "CUSTOM")]
    Custom,

    [EnumMember(Value = "DATABASE")]
    Database,

    [EnumMember(Value = "DOCUMENT")]
    Document,

    [EnumMember(Value = "DRIVER_LICENSE")]
    DriverLicense,

    [EnumMember(Value = "EMAIL_ACCOUNT")]
    EmailAccount,

    [EnumMember(Value = "IDENTITY")]
    Identity,

    [EnumMember(Value = "LOGIN")]
    Login,

    [EnumMember(Value = "MEDICAL_RECORD")]
    MedicalRecord,

    [EnumMember(Value = "MEMBERSHIP")]
    Membership,

    [EnumMember(Value = "OUTDOOR_LICENSE")]
    OutdoorLicense,

    [EnumMember(Value = "PASSPORT")]
    Passport,

    [EnumMember(Value = "PASSWORD")]
    Password,

    [EnumMember(Value = "REWARD_PROGRAM")]
    RewardProgram,

    [EnumMember(Value = "SECURE_NOTE")]
    SecureNote,

    [EnumMember(Value = "SERVER")]
    Server,

    [EnumMember(Value = "SOCIAL_SECURITY_NUMBER")]
    SocialSecurityNumber,

    [EnumMember(Value = "SOFTWARE_LICENSE")]
    SoftwareLicense,

    [EnumMember(Value = "SSH_KEY")]
    SshKey,

    Unknown,

    [EnumMember(Value = "WIRELESS_ROUTER")]
    WirelessRouter
}