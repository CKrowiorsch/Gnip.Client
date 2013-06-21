using System;

namespace Krowiorsch.Gnip.Model.Data
{
    public class ActivityNote : Activity
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