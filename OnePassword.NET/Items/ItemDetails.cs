using System.Text;

namespace OnePassword.Items;

public class ItemDetails
{
    [JsonPropertyName("password")]
    public string Password { get; init; } = "";

    [JsonPropertyName("passwordHistory")]
    public List<PasswordHistory> PasswordHistory { get; init; } = new();

    [JsonPropertyName("fields")]
    public ItemFieldList Fields { get; init; } = new();

    [JsonPropertyName("notesPlain")]
    public string Notes { get; init; } = "";

    [JsonPropertyName("sections")]
    public SectionList Sections { get; init; } = new();

    public string ToBase64() => Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(this, new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull }))).Replace("=", "");
}