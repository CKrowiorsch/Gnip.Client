namespace Krowiorsch.Gnip.Model.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class ActivityVideo : Activity
    {
        public string VideoSubtitle { get; set; }
        
        public string VideoContent { get; set; }
        
        public string VideoSummary { get; set; }

        public string Related { get; set; }

        public string Via { get; set; }

        public string Preview { get; set; }

        public string VideoTitle { get; set; }

        /// <summary> category of the video </summary>
        public string[] Category { get; set; }

        public override string GetContent()
        {
            return VideoContent;
        }
    }
}