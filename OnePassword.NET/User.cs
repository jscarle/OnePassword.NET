using System;
using Newtonsoft.Json;
using OnePassword.Common;
using OnePassword.Users;

namespace OnePassword
{
    public class User
    {
        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("lastAuthAt")]
        public DateTime LastAuthenticationAt { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("state")]
        public State State { get; set; }

        [JsonProperty("type")]
        public UserType UserType { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("accountKeyFormat")]
        public string AccountKeyFormat { get; set; }

        [JsonProperty("accountKeyUuid")]
        public string AccountKeyUuid { get; set; }

        [JsonProperty("attrVersion")]
        public int AttributesVersion { get; set; }

        [JsonProperty("keysetVersion")]
        public int KeysetVersion { get; set; }

        [JsonProperty("combinedPermissions")]
        public long CombinedPermissions { get; set; }
    }
}
