using System.Collections.Generic;
using Newtonsoft.Json;
using OnePassword.Items;

namespace OnePassword
{
    public class Template
    {
        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonIgnore]
        public string Title { get; set; }

        [JsonIgnore]
        public PasswordRecipe? PasswordRecipe { get; set; }

        [JsonIgnore]
        public string Url { get; set; }

        [JsonIgnore]
        public string Filename { get; set; }

        [JsonIgnore]
        public ItemDetails Details { get; set; }

        [JsonIgnore]
        public List<string> Tags { get; set; }

        public Template()
        {
            Tags = new List<string>();
        }
    }
}
