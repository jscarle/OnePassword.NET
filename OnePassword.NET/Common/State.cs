namespace OnePassword.Common;

[JsonConverter(typeof(JsonStringEnumConverterEx<State>))]
public enum State
{
    [EnumMember(Value = "Active")]
    Active,

    [EnumMember(Value = "Inactive")]
    Inactive,

    [EnumMember(Value = "Suspended")]
    Suspended,

    [EnumMember(Value = "Transfer Accepted")]
    TransferAccepted,

    [EnumMember(Value = "Transfer Pending")]
    TransferPending,

    [EnumMember(Value = "Transfer Started")]
    TransferStarted,

    [EnumMember(Value = "Transfer Suspended")]
    TransferSuspended,

    Unknown
}