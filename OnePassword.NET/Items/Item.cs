using OnePassword.Common;

namespace OnePassword.Items;

public class Item
{
    [JsonPropertyName("uuid")]
    public string Uuid { get; set; } = "";

    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; set; }

    [JsonPropertyName("templateUuid")]
    public string TemplateUuid { get; set; } = "";

    [JsonPropertyName("changerUuid")]
    public string ChangerUuid { get; set; } = "";

    [JsonPropertyName("itemVersion")]
    public int? ItemVersion { get; set; }

    [JsonPropertyName("vaultUuid")]
    public string VaultUuid { get; set; } = "";

    [JsonPropertyName("overview")]
    public ItemOverview Overview { get; set; } = new();

    [JsonPropertyName("details")]
    public ItemDetails Details { get; set; } = new();

    [JsonPropertyName("faveIndex")]
    public int? FavoriteIndex { get; set; }

    [JsonConverter(typeof(YesNoJsonConverter))]
    [JsonPropertyName("trashed")]
    public bool? Trashed { get; set; }
}