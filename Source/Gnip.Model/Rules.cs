using Newtonsoft.Json;

namespace Krowiorsch.Gnip.Model
{
    public class Rules
    {
        public Rules()
        {
        }

        public Rules(Rule[] list)
        {
            List = list;
        }

        public Rules(Rule rule)
        {
            List = new Rule[] { rule };
        }

        [JsonProperty("rules")]
        public Rule[] List { get; set; }
    }
}