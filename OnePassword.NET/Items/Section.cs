using Newtonsoft.Json;

namespace OnePassword.Items
{
    public class Section
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("fields")]
        public SectionFieldList Fields { get; set; }

        public Section()
        {
            Fields = new SectionFieldList();
        }
    }
}
