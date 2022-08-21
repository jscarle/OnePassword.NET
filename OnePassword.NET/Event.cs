using System;
using Newtonsoft.Json;
using OnePassword.Events;

namespace OnePassword
{
    public class Event
    {
        [JsonProperty("eid")]
        public int EventId { get; set; }

        [JsonProperty("time")]
        public DateTime Time { get; set; }

        [JsonProperty("ctorUuid")]
        public string ActorUuid { get; set; }

        [JsonProperty("action")]
        public EventAction Action { get; set; }

        [JsonProperty("objectType")]
        public ObjectType ObjectType { get; set; }

        [JsonProperty("objectUuid")]
        public string ObjectUuid { get; set; }

        [JsonProperty("auxInfo")]
        public string AuxiliaryInformation { get; set; }

        [JsonProperty("auxUUID")]
        public string AuxiliaryUuid { get; set; }
    }
}
