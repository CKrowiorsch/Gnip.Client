using Newtonsoft.Json;

namespace Krowiorsch.Model.Gnip
{
    /// <summary> Gnip Specific extension for Activities </summary>
    public class Gnip
    {
        /// <summary> Describes the rules that matched this Activity </summary>
        [JsonProperty("matching_rules")]
        public MatchingRule[] MatchingRules { get; set; }

        /// <summary> additional feature from gnip (Url Expansion) </summary>
        public GnipUrl[] Urls { get; set; }

        /// <summary> addional feature from gnip (Language detection) </summary>
        public GnipLanguage Language { get; set; }
    }
}