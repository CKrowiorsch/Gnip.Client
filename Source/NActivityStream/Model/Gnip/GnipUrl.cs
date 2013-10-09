using Newtonsoft.Json;

namespace Krowiorsch.Model.Gnip
{
    public class GnipUrl
    {
        public string Url { get; set; } 
        
        [JsonProperty("expaned_url")]
        public string ExpandedUrl { get; set; } 
    }
}