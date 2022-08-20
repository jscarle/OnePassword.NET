using System.Text;

namespace OnePassword.Items;

public class ItemDetails
{
    [JsonPropertyName("password")]
    public string Password { get; set; } = "";

    [JsonPropertyName("passwordHistory")]
    public List<PasswordHistory> PasswordHistory { get; set; } = new();

    [JsonPropertyName("fields")]
    public ItemFieldList Fields { get; set; } = new();

    [JsonPropertyName("notesPlain")]
    public string Notes { get; set; } = "";

    [JsonPropertyName("sections")]
    public SectionList Sections { get; set; } = new();

    public string ToBase64() => Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(this, new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull }))).Replace("=", "");
}