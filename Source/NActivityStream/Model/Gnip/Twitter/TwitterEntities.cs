using System.Security.Policy;

namespace Krowiorsch.Model.Gnip.Twitter
{
    public class TwitterEntities
    {
        public Hash[] Hashtags { get; set; }
        
        public object[] Symbols { get; set; }
        
        public TwitterUrl[] Urls { get; set; }
    }
}