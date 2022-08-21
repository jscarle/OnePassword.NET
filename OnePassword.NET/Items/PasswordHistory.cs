using Newtonsoft.Json;

namespace OnePassword.Items
{
    public class PasswordHistory
    {
        [JsonProperty("time")]
        public int Time { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
