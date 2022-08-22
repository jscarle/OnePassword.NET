namespace OnePassword.Items;

public class SectionFieldAdditional
{
    [JsonPropertyName("guarded")]
    public string Guarded { get; init; } = "";

    [JsonPropertyName("multiline")]
    public string Multiline { get; init; } = "";

    [JsonPropertyName("generate")]
    public string Generate { get; init; } = "";

    [JsonPropertyName("clipboardFilter")]
    public string ClipboardFilter { get; init; } = "";
}