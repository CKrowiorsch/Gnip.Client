using Newtonsoft.Json;

namespace Krowiorsch.Model.Gnip.Twitter
{
    public class TwitterActivity : GnipActivity
    {
        [JsonProperty("twitter_entities")]
        public TwitterEntities TwitterEntities { get; set; }

        public int FavoritesCount { get; set; }
        
        public int RetweetCount { get; set; }


        [JsonProperty("twitter_filter_level")]
        public string TwitterFilterLevel { get; set; }

        [JsonProperty("twitter_lang")]
        public string TwitterLanguage { get; set; }
    }
}