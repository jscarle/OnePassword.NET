using OnePassword.Items;

namespace OnePassword.Documents;

public class DocumentDetails : ItemDetails
{
    [JsonPropertyName("documentAttributes")]
    public DocumentAttributes DocumentAttributes { get; init; } = new();
}