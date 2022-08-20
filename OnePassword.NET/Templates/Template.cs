using OnePassword.Items;

namespace OnePassword.Templates;

public class Template
{
    [JsonPropertyName("uuid")]
    public string Uuid { get; set; } = "";

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonIgnore]
    public string Title { get; set; } = "";

    [JsonIgnore]
    public PasswordRecipe? PasswordRecipe { get; set; }

    [JsonIgnore]
    public string Url { get; set; } = "";

    [JsonIgnore]
    public string Filename { get; set; } = "";

    [JsonIgnore]
    public ItemDetails Details { get; set; } = new();

    [JsonIgnore]
    public List<string> Tags { get; set; } = new();
}