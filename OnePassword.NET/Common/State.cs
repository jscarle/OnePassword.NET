using Newtonsoft.Json;

namespace OnePassword.Common
{
    [JsonConverter(typeof(StateConverter))]
    public enum State
    {
        Active,
        Suspended
    }
}
