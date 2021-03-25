using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace OnePassword.Items
{
    public class ItemDetails
    {
        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("passwordHistory")]
        public List<PasswordHistory> PasswordHistory { get; set; }

        [JsonProperty("fields")]
        public ItemFieldList Fields { get; set; }

        [JsonProperty("notesPlain")]
        public string Notes { get; set; }

        [JsonProperty("sections")]
        public SectionList Sections { get; set; }

        public ItemDetails()
        {
            PasswordHistory = new List<PasswordHistory>();
            Fields = new ItemFieldList();
            Sections = new SectionList();
        }

        public string ToBase64() => Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }))).Replace("=", "");
    }
}
