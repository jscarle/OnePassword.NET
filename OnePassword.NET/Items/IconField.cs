namespace OnePassword.Items;

public class IconField
{
    [JsonPropertyName("detail")]
    public IconDetails Details { get; set; } = new();
}