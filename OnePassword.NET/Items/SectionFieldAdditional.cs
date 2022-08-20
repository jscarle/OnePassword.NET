namespace OnePassword.Items;

public class SectionFieldAdditional
{
    [JsonPropertyName("guarded")]
    public string Guarded { get; set; } = "";

    [JsonPropertyName("multiline")]
    public string Multiline { get; set; } = "";

    [JsonPropertyName("generate")]
    public string Generate { get; set; } = "";

    [JsonPropertyName("clipboardFilter")]
    public string ClipboardFilter { get; set; } = "";
}