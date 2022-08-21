using Newtonsoft.Json;
using OnePassword.Documents;

namespace OnePassword
{
    public class Document : Item
    {
        [JsonProperty("details")]
        public new DocumentDetails Details { get; set; }
    }
}