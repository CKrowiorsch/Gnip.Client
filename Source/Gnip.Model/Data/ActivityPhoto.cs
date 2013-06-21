using System;

namespace Krowiorsch.Gnip.Model.Data
{
    public class ActivityPhoto : Activity
    {
        public string PhotoTitle { get; set; }

        public string PhotoSubtitle { get; set; }

        public string PhotoContent { get; set; }

        public string PhotoLink { get; set; }

        public string Related { get; set; }

        public string Via { get; set; }

        public string Preview { get; set; }

        public string InReplyTo { get; set; }

        public override string GetContent()
        {
            return PhotoTitle + Environment.NewLine + PhotoSubtitle + Environment.NewLine + PhotoContent;
        }
    }
}