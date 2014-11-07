using System;

namespace Krowiorsch.Gnip.Model.Data
{
    /// <summary> Baseclass for all activities </summary>
    public class Activity
    {
        /// <summary> unique identifier </summary>
        public string Id { get; set; }

        /// <summary> publishingDate </summary>
        public DateTime Published { get; set; }
        
        /// <summary> Date of last Update </summary>
        public DateTime Updated { get; set; }
        
        public DateTime Created { get; set; }
        
        public DateTime Received { get; set; }

        public string Title { get; set; }
        
        public string Link { get; set; }

        public EntryAuthor Author { get; set; }
        
        public EntryActor Actor { get; set; }

        public MatchingRule[] Rules { get; set; }

        public string ObjectTypeUri { get; set; }

        public string RawXml { get; set; }

        public virtual string GetContent()
        {
            return Title;
        }

    }
}