using OnePassword.Common;

namespace OnePassword.Items;

public class Item
{
    [JsonPropertyName("uuid")]
    public string Uuid { get; init; } = "";

    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; init; }

    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; init; }

    [JsonPropertyName("templateUuid")]
    public string TemplateUuid { get; init; } = "";

    [JsonPropertyName("changerUuid")]
    public string ChangerUuid { get; init; } = "";

    [JsonPropertyName("itemVersion")]
    public int? ItemVersion { get; init; }

    [JsonPropertyName("vaultUuid")]
    public string VaultUuid { get; init; } = "";

    [JsonPropertyName("overview")]
    public ItemOverview Overview { get; init; } = new();

    [JsonPropertyName("details")]
    public ItemDetails Details { get; init; } = new();

    [JsonPropertyName("faveIndex")]
    public int? FavoriteIndex { get; init; }

    [JsonConverter(typeof(YesNoJsonConverter))]
    [JsonPropertyName("trashed")]
    public bool? Trashed { get; init; }
}