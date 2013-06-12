using System;

namespace Krowiorsch.Gnip.Model.Facebook
{
    public class FacebookNote : FacebookRoot
    {
        public string NoteTitle { get; set; }
        
        public string NoteContent { get; set; }

        public string NoteLink { get; set; }

        public string Via { get; set; }

        public int LikeCount { get; set; }

        public override string GetContent()
        {
            return NoteTitle + Environment.NewLine + NoteContent;
        }
    }
}