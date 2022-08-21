using System;
using Newtonsoft.Json;
using OnePassword.Accounts;
using OnePassword.Common;

namespace OnePassword
{
    public class Account
    {
        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public AccountType AccountType { get; set; }

        [JsonProperty("state")]
        public State State { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        [JsonProperty("domain")]
        public string Domain { get; set; }

        [JsonProperty("baseAvatarURL")]
        public string BaseAvatarUrl { get; set; }

        [JsonProperty("baseAttachmentURL")]
        public string BaseAttachmentUrl { get; set; }

        [JsonProperty("attrVersion")]
        public int AttributesVersion { get; set; }
    }
}
