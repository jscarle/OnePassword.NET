using Newtonsoft.Json;
using OnePassword.Items;

namespace OnePassword.Documents
{
    public class DocumentDetails : ItemDetails
    {
        [JsonProperty("documentAttributes")]
        public DocumentAttributes DocumentAttributes { get; set; }
    }
}
