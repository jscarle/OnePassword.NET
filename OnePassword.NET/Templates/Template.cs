using OnePassword.Items;

namespace OnePassword.Templates;

public class Template
{
    [JsonPropertyName("uuid")]
    public string Uuid { get; init; } = "";

    [JsonPropertyName("name")]
    public string Name { get; init; } = "";

    [JsonIgnore]
    public string Title { get; init; } = "";

    [JsonIgnore]
    public PasswordRecipe? PasswordRecipe { get; init; }

    [JsonIgnore]
    public string Url { get; init; } = "";

    [JsonIgnore]
    public string Filename { get; init; } = "";

    [JsonIgnore]
    public ItemDetails Details { get; set; } = new();

    [JsonIgnore]
    public List<string> Tags { get; init; } = new();
}