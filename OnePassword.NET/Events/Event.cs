namespace OnePassword.Events;

public class Event
{
    [JsonPropertyName("eid")]
    public int EventId { get; set; }

    [JsonPropertyName("time")]
    public DateTime Time { get; set; }

    [JsonPropertyName("ctorUuid")]
    public string ActorUuid { get; set; } = "";

    [JsonPropertyName("action")]
    public EventAction Action { get; set; }

    [JsonPropertyName("objectType")]
    public ObjectType ObjectType { get; set; }

    [JsonPropertyName("objectUuid")]
    public string ObjectUuid { get; set; } = "";

    [JsonPropertyName("auxInfo")]
    public string AuxiliaryInformation { get; set; } = "";

    [JsonPropertyName("auxUUID")]
    public string AuxiliaryUuid { get; set; } = "";
}