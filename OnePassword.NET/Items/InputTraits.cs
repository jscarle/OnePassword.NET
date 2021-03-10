using Newtonsoft.Json;

namespace OnePassword.Items
{
    public class InputTraits
    {
        [JsonProperty("autocapitalization")]
        public string AutoCapitalization { get; set; }

        [JsonProperty("autocorrection")]
        public string AutoCorrection { get; set; }

        [JsonProperty("keyboard")]
        public string Keyboard { get; set; }
    }
}
