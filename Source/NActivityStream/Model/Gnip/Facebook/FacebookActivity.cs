using Krowiorsch.Converter.Gnip;

using Newtonsoft.Json;

namespace Krowiorsch.Model.Gnip.Facebook
{
    public class FacebookActivity : Activity
    {
        [JsonConverter(typeof(MatchingRulesConverter))]
        public MatchingRule[] MatchingRules { get; set; }
        
        public FacebookAuthor Author { get; set; }
    }
}