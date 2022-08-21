using Newtonsoft.Json;

namespace OnePassword.Items
{
    public class IconField
    {
        [JsonProperty("detail")]
        public IconDetails Details { get; set; }
    }
}
