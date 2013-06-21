namespace Krowiorsch.Gnip.Model.Data
{
    public class ActivityMusic : Activity
    {
        public string MusicTitle { get; set; }
        
        public string MusicSubTitle { get; set; }
        
        public string MusicContent { get; set; }

        public string Via { get; set; }
        
        public string Enclosure { get; set; }

        public int LikeCount { get; set; }
    }
}