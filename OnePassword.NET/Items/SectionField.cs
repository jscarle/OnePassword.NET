using Newtonsoft.Json;

namespace OnePassword.Items
{
    public class SectionField
    {
        [JsonProperty("n")]
        public string Name { get; set; }

        [JsonProperty("t")]
        public string Title { get; set; }

        [JsonProperty("k")]
        public string FieldType { get; set; }

        [JsonProperty("v")]
        public object Value { get; set; }

        [JsonProperty("a")]
        public SectionFieldAdditional Additional { get; set; }

        [JsonProperty("inputTraits")]
        public InputTraits InputTraits { get; set; }
    }
}
