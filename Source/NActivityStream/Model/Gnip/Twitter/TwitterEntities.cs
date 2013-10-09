using System.Security.Policy;

namespace Krowiorsch.Model.Gnip.Twitter
{
    public class TwitterEntities
    {
        public Hash[] Hashtags { get; set; }
        
        public string[] Symbols { get; set; }
        
        public TwitterUrl[] Urls { get; set; }
    }
}