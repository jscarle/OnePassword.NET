namespace OnePassword.Common;

/// <summary>
/// Represents a state.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverterEx<State>))]
public enum State
{
    /// <summary>
    /// Active
    /// </summary>
    [EnumMember(Value = "Active")]
    Active,

    /// <summary>
    /// Inactive
    /// </summary>
    [EnumMember(Value = "Inactive")]
    Inactive,

    /// <summary>
    /// Suspended
    /// </summary>
    [EnumMember(Value = "Suspended")]
    Suspended,

    /// <summary>
    /// Transfer has been accepted.
    /// </summary>
    [EnumMember(Value = "Transfer Accepted")]
    TransferAccepted,

    /// <summary>
    /// Transfer is pending.
    /// </summary>
    [EnumMember(Value = "Transfer Pending")]
    TransferPending,

    /// <summary>
    /// Transfer has been started.
    /// </summary>
    [EnumMember(Value = "Transfer Started")]
    TransferStarted,

    /// <summary>
    /// Transfer has been suspended.
    /// </summary>
    [EnumMember(Value = "Transfer Suspended")]
    TransferSuspended,

    /// <summary>
    /// The state is unknown.
    /// </summary>
    Unknown
}