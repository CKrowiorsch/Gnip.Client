using System;

namespace Krowiorsch.Gnip.Model.Data
{
    public class ActivityComment : Activity
    {
        public string CommentTitle { get; set; }
        
        public string CommentContent { get; set; }

        public string CommentLink { get; set; }

        public string Via { get; set; }

        public string InReplyTo { get; set; }

        public override string GetContent()
        {
            return CommentTitle + Environment.NewLine + CommentContent;
        }
    }
}