using Newtonsoft.Json;

namespace OnePassword.Items
{
    public class UrlField
    {
        [JsonProperty("l")]
        public string Label { get; set; }

        [JsonProperty("u")]
        public string Url { get; set; }
    }
}
