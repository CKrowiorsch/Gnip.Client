using Newtonsoft.Json;

namespace Krowiorsch.Model.Gnip
{
    /// <summary>
    /// GnipFeature (Resolving Url)
    /// </summary>
    public class GnipUrl
    {
        /// <summary> Url </summary>
        public string Url { get; set; } 
        
        /// <summary> resolved Url </summary>
        [JsonProperty("expanded_url")]
        public string ExpandedUrl { get; set; }

        /// <summary> displayurl </summary>
        [JsonProperty("display_url")]
        public string DisplayUrl { get; set; } 
    }
}