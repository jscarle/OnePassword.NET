using Newtonsoft.Json;

namespace OnePassword.Items
{
    public class SectionFieldAdditional
    {
        [JsonProperty("guarded")]
        public string Guarded { get; set; }

        [JsonProperty("multiline")]
        public string Multiline { get; set; }

        [JsonProperty("generate")]
        public string Generate { get; set; }

        [JsonProperty("clipboardFilter")]
        public string ClipboardFilter { get; set; }
    }
}
