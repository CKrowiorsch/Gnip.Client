using System;

namespace Krowiorsch.Gnip.Model.Facebook
{
    public class FacebookRoot
    {
        public string Id { get; set; }

        public DateTime Published { get; set; }
        
        public DateTime Updated { get; set; }
        
        public DateTime Created { get; set; }
        
        public DateTime Received { get; set; }

        public string Title { get; set; }
        
        public string Link { get; set; }

        public FacebookAuthor Author { get; set; }

        public MatchingRule[] Rules { get; set; }

        public string ObjectTypeUri { get; set; }

        public string RawXml { get; set; }

        public virtual string GetContent()
        {
            return Title;
        }

    }
}