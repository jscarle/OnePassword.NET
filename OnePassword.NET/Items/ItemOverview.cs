using System.Collections.Generic;
using Newtonsoft.Json;

namespace OnePassword.Items
{
    public class ItemOverview
    {
        [JsonProperty("icons")]
        public IconField Icon { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("ainfo")]
        public string AdditionalInfo { get; set; }

        [JsonProperty("pgrng")]
        public bool? PasswordGenerated { get; set; }

        [JsonProperty("pbe")]
        public double? PasswordEntropy { get; set; }

        [JsonProperty("ps")]
        public double? PasswordStrength { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("autosubmit")]
        public string AutoSubmit { get; set; }

        [JsonProperty("URLs")]
        public List<UrlField> Urls { get; set; }

        [JsonProperty("tags")]
        public List<string> Tags { get; set; }

        public ItemOverview()
        {
            Urls = new List<UrlField>();
            Tags = new List<string>();
        }
    }
}