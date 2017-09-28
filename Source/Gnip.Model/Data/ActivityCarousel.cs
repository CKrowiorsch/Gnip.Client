namespace Krowiorsch.Gnip.Model.Data
{
    /// <summary>
    /// Represents two or more graphical image. Objects of this type MAY contain an additional fullImage property whose value is an Activity Streams 
    /// Media Link to a "full-sized" representation of the image.
    /// </summary>
    public class ActivityCarousel : Activity
    {
        /// <summary> texttuell content of the Image </summary>
        public string ImageContent { get; set; }

        /// <summary> Link to HtmlView </summary>
        public string HtmlViewLink { get; set; }

        /// <summary> Link to a Preview </summary>
        public string PreviewLink { get; set; }

        /// <summary> EnclosureLink </summary>
        public string EnclosureLink { get; set; }

        /// <summary> category of the image </summary>
        public string[] Category { get; set; }

        /// <summary> an Activity Streams Media Link to a "full-sized" representation of the image </summary>
        public string FullImage { get; set; }

        /// <summary> Number of Likes in the Image </summary>
        public int LikeCount { get; set; }


        public override string GetContent()
        {
            return ImageContent;
        }
    }
}