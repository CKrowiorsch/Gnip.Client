using System;

namespace Krowiorsch.Gnip.Model.Data
{
    public class ActivityBookmark : Activity
    {
        public string BookmarkTitle { get; set; }

        public string BookmarkSubtitle { get; set; }

        public string BookmarkContent { get; set; }

        public string BookmarkSummary { get; set; }

        public string Related { get; set; }

        public string Via { get; set; }

        public string Preview { get; set; }

        public string InReplyTo { get; set; }

        public int LikeCount { get; set; }

        public override string GetContent()
        {
            return BookmarkTitle + Environment.NewLine + BookmarkSubtitle + Environment.NewLine + BookmarkContent + Environment.NewLine + BookmarkSummary;
        }

    }
}