using OnePassword.Items;

namespace OnePassword.Documents;

public class Document : Item
{
    [JsonPropertyName("details")]
    public new DocumentDetails Details { get; set; } = new();
}