using System;

namespace Krowiorsch.Gnip.Model.Data
{
    public class Activity
    {
        public string Id { get; set; }

        public DateTime Published { get; set; }
        
        public DateTime Updated { get; set; }
        
        public DateTime Created { get; set; }
        
        public DateTime Received { get; set; }

        public string Title { get; set; }
        
        public string Link { get; set; }

        public EntryAuthor Author { get; set; }

        public MatchingRule[] Rules { get; set; }

        public string ObjectTypeUri { get; set; }

        public string RawXml { get; set; }

        public virtual string GetContent()
        {
            return Title;
        }

    }
}