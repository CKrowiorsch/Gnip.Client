using Newtonsoft.Json;

namespace Krowiorsch.Gnip.Model
{
    public class Rule
    {
        [JsonProperty("tag")]
        public string Tag { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}