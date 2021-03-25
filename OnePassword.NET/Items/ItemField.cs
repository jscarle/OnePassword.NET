using Newtonsoft.Json;

namespace OnePassword.Items
{
    public class ItemField
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("designation")]
        public string Designation { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string FieldType { get; set; }
        
        [JsonProperty("value")]
        public object Value { get; set; }
    }
}
