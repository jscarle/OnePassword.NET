using System;
using Newtonsoft.Json;
using OnePassword.Common;
using OnePassword.Items;

namespace OnePassword
{
    public class Item
    {
        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty("templateUuid")]
        public string TemplateUuid { get; set; }

        [JsonProperty("changerUuid")]
        public string ChangerUuid { get; set; }

        [JsonProperty("itemVersion")]
        public int? ItemVersion { get; set; }

        [JsonProperty("vaultUuid")]
        public string VaultUuid { get; set; }

        [JsonProperty("overview")]
        public ItemOverview Overview { get; set; }

        [JsonProperty("details")]
        public ItemDetails Details { get; set; }

        [JsonProperty("faveIndex")]
        public int? FavoriteIndex { get; set; }

        [JsonConverter(typeof(YesNoConverter))]
        [JsonProperty("trashed")]
        public bool? Trashed { get; set; }
    }
}